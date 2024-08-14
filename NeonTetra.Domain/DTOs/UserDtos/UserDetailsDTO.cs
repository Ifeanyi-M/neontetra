using NeonTetra.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTetra.Domain.DTOs.UserDtos
{
    public class UserDetailsDTO
    {
        public List<int>? LanguageIds { get; set; } = new List<int>();
       // public List<Language>? Languages { get; set; }

        public int StateId { get; set; }

        public List<int>? NicheIds { get; set; } = new List<int>();
        //public List<Niche>? Niches { get; set; }

    }
}
