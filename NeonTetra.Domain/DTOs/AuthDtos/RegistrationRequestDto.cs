using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTetra.Domain.DTOs.AuthDtos
{
    public class RegistrationRequestDto
    {
        [Required(ErrorMessage = "First name cannot be blank")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name cannot be blank")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email cannot be blank")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password cannot be blank")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Username cannot be blank")]
        public string UserName { get; set; }


    }
}
