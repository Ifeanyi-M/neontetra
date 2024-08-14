using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTetra.Domain.Entities
{
    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserLanguage> UserLanguages { get; set; } = new List<UserLanguage>();
    }
}
