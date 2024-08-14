using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTetra.Domain.Entities
{
    public class Niche
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserNiche> UserNiches { get; set; } = new List<UserNiche>();
    }
}
