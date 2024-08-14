using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NeonTetra.Data.Repositories.Interfaces;
using NeonTetra.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTetra.Data.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<BankDetails> AddBankDetailsAsync(BankDetails details)
        {
            var bank = await _context.BankDetails.AddAsync(details);
            await _context.SaveChangesAsync();
            return bank.Entity;
        }

        public async Task<UserLanguage> AddtoUserLanguageAsync(UserLanguage language)
        {
            var userLanguage = await _context.UserLanguages.AddAsync(language);
            await _context.SaveChangesAsync();
            return userLanguage.Entity;
        }

        public async Task<UserNiche> AddtoUserNicheAsync(UserNiche niche)
        {
            var userNiche = await _context.UserNiches.AddAsync(niche);
            await _context.SaveChangesAsync();
            return userNiche.Entity;
        }

        public async Task<Bank> GetBankById(int Id)
        {
            return await _context.Banks.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<Gender> GetGenderById(int Id)
        {
            return await _context.Genders.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<Language> GetLanguageById(int Id)
        {
            return await _context.Languages.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<Niche> GetNicheById(int Id)
        {
            return await _context.Niches.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<State> GetStateById(int Id)
        {
            return await _context.States.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var retrievedUser = await _userManager.FindByIdAsync(user.Id);

            if(retrievedUser != null)
            {
                var result = await _userManager.UpdateAsync(retrievedUser);

                if (result.Succeeded)
                {
                    return retrievedUser;
                }
                return null;
            }
            return null;
        }
    }
}
