using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using NeonTetra.Data;
using NeonTetra.Data.Repositories.Interfaces;
using NeonTetra.Domain.Common;
using NeonTetra.Domain.DTOs.AuthDtos;
using NeonTetra.Domain.DTOs.UtilityDtos;
using NeonTetra.Domain.Entities;
using NeonTetra.Services.Interfaces;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTetra.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ApplicationDbContext _applicationDb;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepo;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator,
            ApplicationDbContext applicationDb, IEmailService emailService, IUserRepository userRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _applicationDb = applicationDb;
            _emailService = emailService;
            _userRepo = userRepo;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user =_userManager.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    //create role if it does not exist
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<ResponseDto<bool>> ForgotPassword(ForgotPasswordDTO dTO)
        {
            var errors = new List<Error>();
            try
            {
                var user = await _userManager.FindByEmailAsync(dTO.Email);
                if (user == null)
                {
                    errors.Add(new Error("404", "User doesn't exist"));
                    return ResponseDto<bool>.Failure(errors, 404);
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                user.PasswordResetToken = token;

                await _userRepo.UpdateUserAsync(user);

                var resetLink = $"http://localhost:5000/change-password?token={token}&email={user.Email}";

                var emailToSend = new EmailDTO
                {
                    SenderEmail = user.Email,
                    Subject = "Reset Your Password",
                    Body = $"Hello {user.FirstName},{Environment.NewLine}{Environment.NewLine}" +
                           $"You requested a password reset " +
                           "Please reset your password by clicking the link below" +
                           $"{Environment.NewLine}{Environment.NewLine}" +
                           $"<a href=\"{resetLink}\">link</a>" +
                           "This link will take you to a secure page where you can change your password and continue to enjoy our services. " +
                           $"{Environment.NewLine}{Environment.NewLine}" +
                           "Best regards," +
                           $"{Environment.NewLine}" +
                           "The NeonTetra Team."

                };

                //await _emailService.SendEmailAsync(emailToSend);

                return ResponseDto<bool>.Success(true, "Reset Token sent Successfully", 200);

            }
            catch(Exception ex)
            {
                errors.Add(new Error("500", ex.Message));
                return ResponseDto<bool>.Failure(errors, 500);
            }
            
        }

        public async Task<ResponseDto<LoginResponseDTO>> LoginUser(LoginRequestDTO requestDTO)
        {
            var errors = new List<Error>();
            try
            {
                var user = _userManager.Users.FirstOrDefault(u => u.UserName.ToLower() == requestDTO.UserName.ToLower());

                if(user == null)
                {
                    errors.Add(new Error("404", "User doesn't exist"));
                    return ResponseDto<LoginResponseDTO>.Failure(errors, 404);
                }

                bool userCheckPassword = await _userManager.CheckPasswordAsync(user, requestDTO.Password);

                if (!userCheckPassword)
                {
                    errors.Add(new Error("404", "Wrong Password!"));
                    return ResponseDto<LoginResponseDTO>.Failure(errors, 404);
                }

                var result = await _signInManager.PasswordSignInAsync(user, requestDTO.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var token = _jwtTokenGenerator.GenerateToken(user, roles);

                    var responseFragment = new UserDTO
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Username = user.UserName,
                        Name = $"{user.FirstName} {user.LastName}"
                    };

                    var response = new LoginResponseDTO
                    {
                        User = responseFragment,
                        Token = token
                    };
                    return ResponseDto<LoginResponseDTO>.Success(response, "Login successful!", 200);

                }
                errors.Add(new Error("400", "Something went wrong"));
                return ResponseDto<LoginResponseDTO>.Failure(errors, 400);
            }
            catch(Exception ex)
            {
                errors.Add(new Error("500", ex.Message));
                return ResponseDto<LoginResponseDTO>.Failure(errors, 500);
            }
        }

        public async  Task<ResponseDto<UserDTO>> RegisterUser(RegistrationRequestDto requestDto)
        {

            try
            {
                var user = await _userManager.FindByEmailAsync(requestDto.Email);

                if (user != null)
                {
                    var errors = new List<Error>
                   {
                        new Error("400", "User already exists.")
                   };
                    return ResponseDto<UserDTO>.Failure(errors, 400);
                }

                

                var newUser = new User
                {
                    FirstName = requestDto.FirstName,
                    LastName = requestDto.LastName,
                    Email = requestDto.Email,
                    UserName = requestDto.UserName

                };
                var result = await _userManager.CreateAsync(newUser, requestDto.Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

                    

                    var confirmationUrl = $"https://localhost:7295/api/auth/confirmemail?userId={newUser.Id}&token={Uri.EscapeDataString(token)}";

                    // Send confirmation email
                    await _emailService.SendConfirmationEmail(newUser.Email, newUser.FirstName, newUser.LastName, confirmationUrl);

                    await AssignRole(newUser.Email, "Regular");

                    await _emailService.RegisterUserAndSendWelcomeEmail(newUser.Email, newUser.FirstName, newUser.LastName);

                    var returnUser = new UserDTO
                    {
                        Id = newUser.Id,
                        Email = newUser.Email,
                        Username = newUser.UserName,
                        Name = $"{newUser.FirstName} {newUser.LastName}"
                    };

                    return ResponseDto<UserDTO>.Success(returnUser, "Successfully Registered New User", 201);
                }
                return ResponseDto<UserDTO>.Failure(result.Errors.Select(e => new Error(e.Code, e.Description)), 500);

            }
            catch(Exception ex)
            {
                var errors = new List<Error> { new Error("500", ex.Message) };
                return ResponseDto<UserDTO>.Failure(errors, 500);
            }
        }

        public async Task<ResponseDto<string>> ConfirmEmail(string userId, string token)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    var errors = new List<Error> { new Error("404", "User not found.") };
                    return ResponseDto<string>.Failure(errors, 404);
                }

                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return ResponseDto<string>.Success("Email confirmed successfully", "Email confirmed", 200);
                }
                return ResponseDto<string>.Failure(result.Errors.Select(e => new Error(e.Code, e.Description)), 400);
            }
            catch (Exception ex)
            {
                var errors = new List<Error> { new Error("500", ex.Message) };
                return ResponseDto<string>.Failure(errors, 500);
            }
        }

        public async Task<ResponseDto<string>> ResetPassword(ResetPasswordDTO dTO)
        {
            try
            {
                if (dTO.NewPassword != dTO.ConfirmPassword)
                {
                    var errors = new List<Error> { new Error("400", "Passwords do not match.") };
                    return ResponseDto<string>.Failure(errors, 400);
                }

                var user = await _userManager.FindByEmailAsync(dTO.Email);
                if (user == null)
                {
                    return ResponseDto<string>.Failure(new List<Error> { new Error("404", "User not found.") }, 404);
                }

                var result = await _userManager.ResetPasswordAsync(user, dTO.Token, dTO.NewPassword);
                if (result.Succeeded)
                {
                    return ResponseDto<string>.Success("Password reset successfully.", 200);
                }
                return ResponseDto<string>.Failure(result.Errors.Select(e => new Error(e.Code, e.Description)).ToList(), 400);
            }
            catch (Exception ex)
            {
                var errors = new List<Error> { new Error("500", ex.Message) };
                return ResponseDto<string>.Failure(errors, 500);
            }
        }

        private async Task<User> FindUserByPasswordResetTokenAsync(string token)
        {
            // Find the user associated with the given token
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == token);

            if (user == null)
            {
                throw new ApplicationException("User not found for the provided token.");
            }

            // Optionally, you can check token expiration or any other conditions here
            // For example, ensure the token is not expired before returning the user

            return user;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            return users;
        }

    }
}
