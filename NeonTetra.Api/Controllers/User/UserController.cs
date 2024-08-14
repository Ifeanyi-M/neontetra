using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NeonTetra.Domain.DTOs.UserDtos;
using NeonTetra.Services.Interfaces;

namespace NeonTetra.Api.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut("basic-info")]

        public async Task<IActionResult> UpdateBasicInfo(UserBasicInfoDTO dTO)
        {
            var result = await _userService.UpdateUserBasicInfo(dTO);
            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("details")]

        public async Task<IActionResult> UpdateDetails(UserDetailsDTO dTO)
        {
            var result = await _userService.UpdateUserDetails(dTO);
            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("set-rates")]

        public async Task<IActionResult> UpdateUserRates(UserSetRateDTO dTO)
        {
            var result = await _userService.UpdateUserRates(dTO);
            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("add-bank-details")]

        public async Task<IActionResult> AddBankDetails(UserBankDetailsDTO dTO)
        {
            var result = await _userService.AddUserBankDetails(dTO);
            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }


    }
}
