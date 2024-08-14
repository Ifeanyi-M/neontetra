using NeonTetra.Domain.Common;
using NeonTetra.Domain.DTOs.AuthDtos;
using NeonTetra.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTetra.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDto<UserDTO>> RegisterUser(RegistrationRequestDto requestDto);
        Task<ResponseDto<LoginResponseDTO>> LoginUser(LoginRequestDTO requestDTO);
        Task<ResponseDto<bool>> ForgotPassword(ForgotPasswordDTO dTO);

        Task<List<User>> GetAllUsers();

        Task<ResponseDto<string>> ConfirmEmail(string userId, string token);
        Task<ResponseDto<string>> ResetPassword(ResetPasswordDTO dTO);
        Task<bool> AssignRole(string email, string roleName);
    }
}
