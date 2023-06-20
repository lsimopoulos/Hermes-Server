using Hermes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;


namespace Hermes.Data
{
    public class HermesUsersConfiguration : IEntityTypeConfiguration<HermesUser>
    {

        public HermesUsersConfiguration()
        {
        }
        public void Configure(EntityTypeBuilder<HermesUser> builder)
        {
            builder.ToTable("HermesUsers");

            builder
          .HasMany(u => u.Contacts)
          .WithMany()
          .UsingEntity<Dictionary<Guid, Guid>>(
              "HermesUserContacts",
              j => j.HasOne<HermesUser>().WithMany().HasForeignKey("HermesUserId"),
              j => j.HasOne<HermesUser>().WithMany().HasForeignKey("ContactId"),
              j => j.HasKey("HermesUserId", "ContactId")
          );

            builder
                .HasMany(u => u.Groups)
                .WithMany()
                .UsingEntity<Dictionary<Guid, Guid>>(
                    "HermesUserGroups",
                    j => j.HasOne<HermesUser>().WithMany().HasForeignKey("HermesUserId"),
                    j => j.HasOne<HermesUser>().WithMany().HasForeignKey("GroupId"),
                    j => j.HasKey("HermesUserId", "GroupId")
                );
        }
    }
}
