using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NeonTetra.Data.Repositories.Interfaces;
using NeonTetra.Domain.Common;
using NeonTetra.Domain.DTOs.AuthDtos;
using NeonTetra.Domain.DTOs.UserDtos;
using NeonTetra.Domain.Entities;
using NeonTetra.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTetra.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _manager;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<User> manager, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _manager = manager;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseDto<UserUpdateResponse>> AddUserBankDetails(UserBankDetailsDTO dTO)
        {
            var errors = new List<Error>();
            try
            {
                var user = await _manager.GetUserAsync(_httpContextAccessor.HttpContext.User);

                if (user is null)
                {
                    errors.Add(new Error("404", "User doesn't exist"));
                    return ResponseDto<UserUpdateResponse>.Failure(errors, 404);
                }

                var bank = await _userRepository.GetBankById(dTO.BankId);

                var newBankDetails = new BankDetails
                {
                    Bank = bank,
                    BankId = bank.Id,
                    AccountName = dTO.AccountName,
                    AccountNumber = dTO.AccountNumber,
                    User = user,
                    UserId = user.Id
                };

                var addedBankDetails = await _userRepository.AddBankDetailsAsync(newBankDetails);

                var response = new UserUpdateResponse
                {
                    Id = user.Id,
                    Name = $"{user.FirstName} {user.LastName}",
                    Email = user.Email,
                    Username = user.UserName,
                    BirthDate = user.BirthDate,
                    DisplayName = user.DisplayName,
                    Gender = user.Gender is not null ? user.Gender.Name : "",
                    PhoneNumber = user.PhoneNumber,
                    ShortBio = user.ShortBio,
                    State = user.State is not null ? user.State.Name : "",
                    Languages = user.UserLanguages.Select(x => x.Language).Select(x => x.Name).ToList(),
                    Niches = user.UserNiches.Select(x => x.Niche).Select(x => x.Name).ToList(),
                    Price = user.Price,
                    AdditonalHookPrice = user.AdditonalHookPrice,
                    DeliveryTime = user.DeliveryTime,
                    AccountName = addedBankDetails.AccountName,
                    AccountNumber = addedBankDetails.AccountNumber,
                    Bank = bank.Name

                };

                return ResponseDto<UserUpdateResponse>.Success(response, "Bank detail creation successful!", 201);
            }
            catch(Exception ex)
            {
                errors.Add(new Error("500", ex.Message));
                return ResponseDto<UserUpdateResponse>.Failure(errors, 500);
            }
        }

        public async Task<ResponseDto<UserUpdateResponse>> UpdateUserBasicInfo(UserBasicInfoDTO dTO)
        {
            var errors = new List<Error>();
            try
            {
                var user = await _manager.GetUserAsync(_httpContextAccessor.HttpContext.User);

                if(user is null)
                {
                    errors.Add(new Error("404", "User doesn't exist"));
                    return ResponseDto<UserUpdateResponse>.Failure(errors, 404);
                }

                var gender = await _userRepository.GetGenderById(dTO.GenderId);

                user.DisplayName = dTO.DisplayName;
                user.PhoneNumber = dTO.PhoneNumber;
                user.BirthDate = dTO.BirthDate;
                user.Gender = gender is not null ? gender : null;
                user.GenderId = gender is not null ? gender.Id : null;
                user.ShortBio = dTO.ShortBio;

                var updatedUser = await _userRepository.UpdateUserAsync(user);

                var response = new UserUpdateResponse
                {
                    Id = updatedUser.Id,
                    Name = $"{updatedUser.FirstName} {updatedUser.LastName}",
                    Email = updatedUser.Email,
                    Username = updatedUser.UserName,
                    BirthDate = updatedUser.BirthDate,
                    DisplayName = updatedUser.DisplayName,
                    Gender = updatedUser.Gender is not null ?  updatedUser.Gender.Name : "",
                    PhoneNumber = updatedUser.PhoneNumber,
                    ShortBio = updatedUser.ShortBio
                };

                return ResponseDto<UserUpdateResponse>.Success(response, "Update successful!", 200);

            }
            catch(Exception ex)
            {
                errors.Add(new Error("500", ex.Message));
                return ResponseDto<UserUpdateResponse>.Failure(errors, 500);
            }
        }

        public async Task<ResponseDto<UserUpdateResponse>> UpdateUserDetails(UserDetailsDTO dTO)
        {
            var errors = new List<Error>();
            try
            {
                var user = await _manager.GetUserAsync(_httpContextAccessor.HttpContext.User);

                if (user is null)
                {
                    errors.Add(new Error("404", "User doesn't exist"));
                    return ResponseDto<UserUpdateResponse>.Failure(errors, 404);
                }

                var languageList = new List<string>();

                var list1 = new List<UserLanguage>();

                foreach(int id in dTO.LanguageIds)
                {
                    var language = await _userRepository.GetLanguageById(id);

                    var addUserLanguage = new UserLanguage
                    {
                        UserId = user.Id,
                        User = user,
                        Language = language,
                        LanguageId = language.Id
                    };

                    await _userRepository.AddtoUserLanguageAsync(addUserLanguage);

                    languageList.Add(addUserLanguage.Language.Name);
                    list1.Add(addUserLanguage);
                }

                var nicheList = new List<string>();
                var list2 = new List<UserNiche>();

                foreach (int id in dTO.NicheIds)
                {
                    var niche = await _userRepository.GetNicheById(id);

                    var addUserNiche = new UserNiche
                    {
                        UserId = user.Id,
                        User = user,
                        Niche = niche,
                        NicheId = niche.Id
                    };

                    await _userRepository.AddtoUserNicheAsync(addUserNiche);

                    nicheList.Add(addUserNiche.Niche.Name);
                    list2.Add(addUserNiche);
                }

                var state = await _userRepository.GetStateById(dTO.StateId);

                user.State = state;
                user.StateId = state.Id;
                user.UserLanguages = list1;
                user.UserNiches = list2;

                var updatedUser = await _userRepository.UpdateUserAsync(user);

                

                var response = new UserUpdateResponse
                {
                    Id = updatedUser.Id,
                    Name = $"{updatedUser.FirstName} {updatedUser.LastName}",
                    Email = updatedUser.Email,
                    Username = updatedUser.UserName,
                    BirthDate = updatedUser.BirthDate,
                    DisplayName = updatedUser.DisplayName,
                    Gender = user.Gender is not null ? updatedUser.Gender.Name : "",
                    PhoneNumber = updatedUser.PhoneNumber,
                    ShortBio = updatedUser.ShortBio,
                    State = updatedUser.State is not null ? updatedUser.State.Name : "",
                    Languages = languageList,
                    Niches = nicheList


                };

                return ResponseDto<UserUpdateResponse>.Success(response, "Update successful!", 200);

            }
            catch(Exception ex)
            {
                errors.Add(new Error("500", ex.Message));
                return ResponseDto<UserUpdateResponse>.Failure(errors, 500);
            }
        }

        public async Task<ResponseDto<UserUpdateResponse>> UpdateUserRates(UserSetRateDTO dTO)
        {
            var errors = new List<Error>();

            try
            {
                var user = await _manager.GetUserAsync(_httpContextAccessor.HttpContext.User);

                if (user is null)
                {
                    errors.Add(new Error("404", "User doesn't exist"));
                    return ResponseDto<UserUpdateResponse>.Failure(errors, 404);
                }

                user.Price = dTO.Price;
                user.AdditonalHookPrice = dTO.AdditionalHookPrice;
                user.DeliveryTime = dTO.DeliveryTime;

                var updatedUser = await _userRepository.UpdateUserAsync(user);

                var response = new UserUpdateResponse()
                {
                    Id = user.Id,
                    Name = $"{user.FirstName} {user.LastName}",
                    Email = user.Email,
                    Username = user.UserName,
                    BirthDate = user.BirthDate,
                    DisplayName = user.DisplayName,
                    Gender = user.Gender is not null ? updatedUser.Gender.Name : "",
                    PhoneNumber = user.PhoneNumber,
                    ShortBio = user.ShortBio,
                    State = user.State is not null ? updatedUser.State.Name : "",
                    Languages = user.UserLanguages.Select(x => x.Language).Select(x => x.Name).ToList(),
                    Niches = user.UserNiches.Select(x => x.Niche).Select(x => x.Name).ToList(),
                    Price = updatedUser.Price,
                    AdditonalHookPrice = updatedUser.AdditonalHookPrice,
                    DeliveryTime = updatedUser.DeliveryTime
                };

                return ResponseDto<UserUpdateResponse>.Success(response, "Update successful!", 200);


            }
            catch(Exception ex)
            {
                errors.Add(new Error("500", ex.Message));
                return ResponseDto<UserUpdateResponse>.Failure(errors, 500);
            }
        }
    }
}
