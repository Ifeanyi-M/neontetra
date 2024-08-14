using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTetra.Domain.DTOs.UserDtos
{
    public class UserBasicInfoDTO
    {
        public string? DisplayName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public int GenderId { get; set; }
        public string? ShortBio { get; set; }
    }
}
