using MailChimp.Net;
using MailChimp.Net.Interfaces;
using MailChimp.Net.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using NeonTetra.Domain;
using NeonTetra.Domain.DTOs.UtilityDtos;
using NeonTetra.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTetra.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly string _apiKey;
        private readonly string _listId;
        private readonly string _campaignId;
        private readonly string _confirmId;

        private IMailChimpManager _mailChimpManager;

        public EmailService(IOptions<MailChimpSettings> mailChimpSettings)
        {
            _apiKey = mailChimpSettings.Value.ApiKey;
            _listId = mailChimpSettings.Value.ListId;
            _campaignId = mailChimpSettings.Value.WelcomeCampaign;
            _confirmId = mailChimpSettings.Value.ConfirmId;
            _mailChimpManager = new MailChimpManager(_apiKey);
        }

        //public async Task AddSubscriberUser(string email)
        //{
        //    var member = new Member
        //    {
        //        EmailAddress = email,
        //        StatusIfNew = Status.Subscribed 
        //    };

        //    await _mailChimpManager.Members.AddOrUpdateAsync(_listId, member);
        //}

        public async Task RegisterUserAndSendWelcomeEmail(string email, string firstName, string lastName)
        {
            try
            {
                var member = new Member
                {
                    EmailAddress = email,
                    Status = Status.Subscribed,
                    MergeFields = new System.Collections.Generic.Dictionary<string, object>
                   {
                    { "FNAME", firstName },
                    { "LNAME", lastName }
                   }
                };

                // Add the member to the list
                await _mailChimpManager.Members.AddOrUpdateAsync(_listId, member);

                // Send the existing campaign
                await SendExistingCampaign();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error registering user and sending welcome email: {ex.Message}");
            }
            
        }



        private async Task SendExistingCampaign()
        {
            try
            {
                // Send the existing campaign
                await _mailChimpManager.Campaigns.SendAsync(_campaignId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending campaign: {ex.Message}");
            }
        }


        public async Task<IEnumerable<Campaign>> GetAllCampaignsAsync()
        {
            try
            {
                var campaigns = await _mailChimpManager.Campaigns.GetAllAsync();
                return campaigns;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching campaigns: {ex.Message}");
                return Enumerable.Empty<Campaign>();
            }
        }

        public async Task SendConfirmationEmail(string email, string firstName, string lastName, string confirmationUrl)
        {
            try
            {
                var member = new Member
                {
                    EmailAddress = email,
                    Status = Status.Subscribed,
                    MergeFields = new Dictionary<string, object>
               {
                { "FNAME", firstName },
                { "LNAME", lastName },
                 { "CONFIRMATION_URL", confirmationUrl }
               }
                };

                // Add the member to the list
                await _mailChimpManager.Members.AddOrUpdateAsync(_listId, member);

                // Send the confirmation campaign
                await SendConfirmationCampaign();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending confirmation email: {ex.Message}");
            }
        }

        private async Task SendConfirmationCampaign()
        {
            try
            {
                // Send the confirmation campaign
                await _mailChimpManager.Campaigns.SendAsync(_confirmId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending confirmation campaign: {ex.Message}");
            }
        }
    }
}
