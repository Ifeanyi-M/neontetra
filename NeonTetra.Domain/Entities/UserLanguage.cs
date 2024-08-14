using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTetra.Domain.Entities
{
    public class UserLanguage
    {
        public string? UserId { get; set; }
        public User? User { get; set; }
        public int? LanguageId { get; set; }
        public Language? Language { get; set; }
    }
}
