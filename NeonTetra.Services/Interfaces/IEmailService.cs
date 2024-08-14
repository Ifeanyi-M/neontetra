using MailChimp.Net.Models;
using NeonTetra.Domain.DTOs.UtilityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTetra.Services.Interfaces
{
    public interface IEmailService
    {
        //Task SendEmailAsync(EmailDTO dTO);

        //Task AddSubscriberUser(string email);

        Task RegisterUserAndSendWelcomeEmail(string email, string firstName, string lastName);
        Task<IEnumerable<Campaign>> GetAllCampaignsAsync();
        Task SendConfirmationEmail(string email, string firstName, string lastName, string confirmationUrl);
    }
}
