using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NeonTetra.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTetra.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Gender> Genders { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Niche> Niches { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<UserLanguage> UserLanguages { get; set; }
        public DbSet<UserNiche> UserNiches { get; set; }
        public DbSet<BankDetails> BankDetails { get; set; }

        public DbSet<Bank> Banks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<State>().HasData(
           new State { Id = 1, Name = "Abia" },
           new State { Id = 2, Name = "Adamawa" },
           new State { Id = 3, Name = "Akwa Ibom" },
           new State { Id = 4, Name = "Anambra" },
           new State { Id = 5, Name = "Bauchi" },
           new State { Id = 6, Name = "Bayelsa" },
           new State { Id = 7, Name = "Benue" },
           new State { Id = 8, Name = "Borno" },
           new State { Id = 9, Name = "Cross River" },
           new State { Id = 10, Name = "Delta" },
           new State { Id = 11, Name = "Ebonyi" },
           new State { Id = 12, Name = "Edo" },
           new State { Id = 13, Name = "Ekiti" },
           new State { Id = 14, Name = "Enugu" },
           new State { Id = 15, Name = "Gombe" },
           new State { Id = 16, Name = "Imo" },
           new State { Id = 17, Name = "Jigawa" },
           new State { Id = 18, Name = "Kaduna" },
           new State { Id = 19, Name = "Kano" },
           new State { Id = 20, Name = "Katsina" },
           new State { Id = 21, Name = "Kebbi" },
           new State { Id = 22, Name = "Kogi" },
           new State { Id = 23, Name = "Kwara" },
           new State { Id = 24, Name = "Lagos" },
           new State { Id = 25, Name = "Nasarawa" },
           new State { Id = 26, Name = "Niger" },
           new State { Id = 27, Name = "Ogun" },
           new State { Id = 28, Name = "Ondo" },
           new State { Id = 29, Name = "Osun" },
           new State { Id = 30, Name = "Oyo" },
           new State { Id = 31, Name = "Plateau" },
           new State { Id = 32, Name = "Rivers" },
           new State { Id = 33, Name = "Sokoto" },
           new State { Id = 34, Name = "Taraba" },
           new State { Id = 35, Name = "Yobe" },
           new State { Id = 36, Name = "Zamfara" },
           new State { Id = 37, Name = "Federal Capital Territory" }
             );

            // Seed Genders
            modelBuilder.Entity<Gender>().HasData(
                new Gender { Id = 1, Name = "Male" },
                new Gender { Id = 2, Name = "Female" },
                new Gender { Id = 3, Name = "Non-binary" },
                new Gender { Id = 4, Name = "Other" },
                new Gender { Id = 5, Name = "Prefer not to say" }
            );

            // Seed Languages
            modelBuilder.Entity<Language>().HasData(
                new Language { Id = 1, Name = "Hausa" },
                new Language { Id = 2, Name = "Yoruba" },
                new Language { Id = 3, Name = "Igbo" },
                new Language { Id = 4, Name = "Nigerian Pidgin" }
            );

            // Seed Niches
            modelBuilder.Entity<Niche>().HasData(
                new Niche { Id = 1, Name = "Blogging" },
                new Niche { Id = 2, Name = "Vlogging" },
                new Niche { Id = 3, Name = "Podcasting" },
                new Niche { Id = 4, Name = "Social Media Influencing" },
                new Niche { Id = 5, Name = "Photography" },
                new Niche { Id = 6, Name = "Videography" },
                new Niche { Id = 7, Name = "Graphic Design" },
                new Niche { Id = 8, Name = "Writing" }
            );

            modelBuilder.Entity<Bank>().HasData(
            new Bank { Id = 1, Name = "Access Bank" },
            new Bank { Id = 2, Name = "Citibank" },
            new Bank { Id = 3, Name = "Diamond Bank" },
            new Bank { Id = 4, Name = "Ecobank" },
            new Bank { Id = 5, Name = "Fidelity Bank" },
            new Bank { Id = 6, Name = "First Bank of Nigeria" },
            new Bank { Id = 7, Name = "First City Monument Bank" },
            new Bank { Id = 8, Name = "Guaranty Trust Bank" },
            new Bank { Id = 9, Name = "Heritage Bank" },
            new Bank { Id = 10, Name = "Keystone Bank" },
            new Bank { Id = 11, Name = "Polaris Bank" },
            new Bank { Id = 12, Name = "Providus Bank" },
            new Bank { Id = 13, Name = "Stanbic IBTC Bank" },
            new Bank { Id = 14, Name = "Standard Chartered Bank" },
            new Bank { Id = 15, Name = "Sterling Bank" },
            new Bank { Id = 16, Name = "Suntrust Bank" },
            new Bank { Id = 17, Name = "Union Bank" },
            new Bank { Id = 18, Name = "United Bank for Africa" },
            new Bank { Id = 19, Name = "Unity Bank" },
            new Bank { Id = 20, Name = "Wema Bank" },
            new Bank { Id = 21, Name = "Zenith Bank" }
            );

            // One-to-Many: User to Gender
            modelBuilder.Entity<User>()
                .HasOne(u => u.Gender)
                .WithMany()
                .HasForeignKey(u => u.GenderId);

            // One-to-Many: User to State
            modelBuilder.Entity<User>()
                .HasOne(u => u.State)
                .WithMany()
                .HasForeignKey(u => u.StateId);

            // Many-to-Many: User to Languages
            modelBuilder.Entity<UserLanguage>()
                .HasKey(ul => new { ul.UserId, ul.LanguageId });
            modelBuilder.Entity<UserLanguage>()
                .HasOne(ul => ul.User)
                .WithMany(u => u.UserLanguages)
                .HasForeignKey(ul => ul.UserId);
            modelBuilder.Entity<UserLanguage>()
                .HasOne(ul => ul.Language)
                .WithMany(l => l.UserLanguages)
                .HasForeignKey(ul => ul.LanguageId);

            // Many-to-Many: User to Niches
            modelBuilder.Entity<UserNiche>()
                .HasKey(un => new { un.UserId, un.NicheId });
            modelBuilder.Entity<UserNiche>()
                .HasOne(un => un.User)
                .WithMany(u => u.UserNiches)
                .HasForeignKey(un => un.UserId);
            modelBuilder.Entity<UserNiche>()
                .HasOne(un => un.Niche)
                .WithMany(n => n.UserNiches)
                .HasForeignKey(un => un.NicheId);
        }
    }
}

