using Hermes.Data;
using Hermes.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Hermes.Context
{
    public class HermesContext : IdentityDbContext<HermesUser, IdentityRole<Guid>, Guid>
    {

        public HermesContext(DbContextOptions<HermesContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(true);
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<IdentityRole<Guid>>().ToTable("HermesRoles", "hermesschema");
            //builder.Entity<IdentityUserToken<Guid>>().ToTable("HermesUserTokens", "hermesschema");
            ////builder.Entity<IdentityUserRole<int>>().ToTable("HermesUserRoles", "hermesschema");
            //builder.Entity<IdentityRoleClaim<int>>().ToTable("HermesRoleClaims", "hermesschema");
            //builder.Entity<IdentityUserClaim<int>>().ToTable("HermesUserClaims", "hermesschema");
            //builder.Entity<IdentityUserLogin<Guid>>().ToTable("HermesUserLogins", "hermesschema");

            //builder.Entity<IdentityUserRole<Guid>>().ToTable("HermesUserRoles");
            //builder.Entity<IdentityUserClaim<int>>().ToTable("HermesUserClaims");
            //builder.Entity<IdentityRoleClaim<int>>().ToTable("HermesRoleClaims");
            //builder.Entity<IdentityUserLogin<Guid>>().HasNoKey().ToTable("HermesUserLogins");
            //builder.Entity<IdentityUserToken<Guid>>().ToTable("HermesUserTokens");





            builder.ApplyConfiguration(new HermesUsersConfiguration());
            builder.ApplyConfiguration(new HermesMessagesConfiguration());
            SeedUsersAndContacts.Seed(builder);

        }
        public DbSet<HermesMessage> HermesMessages { get; set; }
    }
}
