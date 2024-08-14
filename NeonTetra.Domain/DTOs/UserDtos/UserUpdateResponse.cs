using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTetra.Domain.DTOs.UserDtos
{
    public class UserUpdateResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string? DisplayName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }
        public string? ShortBio { get; set; }
        public string? State { get; set; }
        public List<string>? Languages { get; set; }
        public List<string>? Niches { get; set; }

        public double? Price { get; set; }
        public double? AdditonalHookPrice { get; set; }
        public string? DeliveryTime { get; set; }

        public string? Bank { get; set; }
        public string? AccountName { get; set; }
        public string? AccountNumber { get; set; }

    }
}
