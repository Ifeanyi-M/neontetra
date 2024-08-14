using NeonTetra.Domain.Common;
using NeonTetra.Domain.DTOs.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTetra.Services.Interfaces
{
    public interface IUserService
    {
        Task<ResponseDto<UserUpdateResponse>> UpdateUserBasicInfo(UserBasicInfoDTO dTO);
        Task<ResponseDto<UserUpdateResponse>> UpdateUserDetails(UserDetailsDTO dTO);
        Task<ResponseDto<UserUpdateResponse>> UpdateUserRates(UserSetRateDTO dTO);

        Task<ResponseDto<UserUpdateResponse>> AddUserBankDetails(UserBankDetailsDTO dTO);
    }
}
