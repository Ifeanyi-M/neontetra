using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTetra.Domain.DTOs.AuthDtos
{
    public class ResetPasswordDTO
    {
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
       public string Token { get; set; }
    }
}
