using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTetra.Domain
{
    public class MailChimpSettings
    {
        public string? ApiKey { get; set; }
        public string? ListId { get; set; }
        public string? WelcomeCampaign { get; set; }
        public string? ConfirmId { get; set; }

    }
}
