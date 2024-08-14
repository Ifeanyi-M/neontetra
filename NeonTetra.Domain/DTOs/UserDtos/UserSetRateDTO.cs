using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTetra.Domain.DTOs.UserDtos
{
    public class UserSetRateDTO
    {
        public double? Price { get; set; }
        public double? AdditionalHookPrice { get; set; }

        public string? DeliveryTime { get; set; }
       
    }
}
