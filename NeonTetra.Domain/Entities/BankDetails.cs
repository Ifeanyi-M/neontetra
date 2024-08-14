using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTetra.Domain.Entities
{
    public class BankDetails
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        //public string Bank { get; set; }
        public string AccountNumber { get; set; }

        public int BankId { get; set; }
        public Bank? Bank { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
    }
}
