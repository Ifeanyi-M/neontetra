using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTetra.Domain.DTOs.UserDtos
{
    public class UserBankDetailsDTO
    {
        public int BankId { get; set; }

        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
    }
}
