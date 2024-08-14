using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTetra.Domain.Entities
{
    public class UserNiche
    {
        public string? UserId { get; set; }
        public User? User { get; set; }
        public int? NicheId { get; set; }
        public Niche? Niche { get; set; }
    }
}
