using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTetra.Domain.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? DisplayName { get; set; }
        public DateTime? BirthDate { get; set; }

        public string? ShortBio { get; set; }
        public double? Price { get; set; }
        public double? AdditonalHookPrice { get; set; }
        public string? DeliveryTime { get; set; }
        public List<UserLanguage>UserLanguages { get; set; } = new List<UserLanguage>();
        public List<UserNiche> UserNiches { get; set; } = new List<UserNiche>();
        public int? StateId { get; set; }
        public State? State { get; set; }

        public int? GenderId { get; set; }
        public Gender? Gender { get; set; }
        public string? PasswordResetToken { get; set; }
    }
}
