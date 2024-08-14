using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTetra.Domain.DTOs.AuthDtos
{
    public class ForgotPasswordDTO
    {
        [Required(ErrorMessage = "Email cannot be blank")]
        public string Email { get; set; }
    }
}
