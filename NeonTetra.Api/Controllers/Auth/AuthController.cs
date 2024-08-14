using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NeonTetra.Domain.DTOs.AuthDtos;
using NeonTetra.Services.Interfaces;

namespace NeonTetra.Api.Controllers.Auth
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> Register(RegistrationRequestDto requestDto)
        {
            var result = await _authService.RegisterUser(requestDto);
            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole(string email, string roleName)
        {
            var result = await _authService.AssignRole(email, roleName);
            if (result == true)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO dTO)
        {
            var result = await _authService.LoginUser(dTO);
            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO dTO)
        {
            var result = await _authService.ForgotPassword(dTO);
            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("confirmemail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var response = await _authService.ConfirmEmail(userId, token);
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDto)
        {
            var response = await _authService.ResetPassword(resetPasswordDto);
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _authService.GetAllUsers();
            
            
                return Ok(result);
            
          
        }
    }
}
