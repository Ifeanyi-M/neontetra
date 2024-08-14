using NeonTetra.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTetra.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> UpdateUserAsync(User user);
        Task<Gender> GetGenderById(int Id);
        Task<Language> GetLanguageById(int Id);
        Task<Niche> GetNicheById(int Id);
        Task<State> GetStateById(int Id);
        Task<Bank> GetBankById(int Id);
        Task<UserNiche> AddtoUserNicheAsync(UserNiche niche);
        Task<UserLanguage> AddtoUserLanguageAsync(UserLanguage language);
        Task<BankDetails> AddBankDetailsAsync(BankDetails details);

    }
}
