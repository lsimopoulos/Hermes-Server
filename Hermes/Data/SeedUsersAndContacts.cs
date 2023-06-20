using Hermes.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hermes.Data
{
    public static class SeedUsersAndContacts
    {
        public static void Seed(ModelBuilder builder)
        {
            var passwordHasher = new PasswordHasher<HermesUser>();
            var listUsers = new List<HermesUser>();
            for (int i = 0; i < 5; i++)
            {
                var hm = new HermesUser { Name = $"haha{i}", UserName = $"haha{i}@haha.com", NormalizedUserName = $"haha{i}@haha.com", SecurityStamp = Guid.NewGuid().ToString() };
                hm.PasswordHash = passwordHasher.HashPassword(hm, "Haha1@#¤");
                listUsers.Add(hm);
                builder.Entity("HermesUserContacts").HasData(new { HermesUserId = hm.Id, ContactId = hm.Id });
            }

            var testGroup = new HermesUser
            {
                Name = "testGroup",
                Email = "testGroup@haha.com",
                UserName = "testGroup@haha.com",
                SecurityStamp = Guid.NewGuid().ToString(),
                IsGroup = true,
            };


            builder.Entity<HermesUser>().HasData(testGroup);

            foreach (var user in listUsers)
            {
                foreach (var contact in listUsers.Where(x => x.Id != user.Id))
                {
                    builder.Entity("HermesUserContacts").HasData(new { HermesUserId = user.Id, ContactId = contact.Id });
                }

                builder.Entity("HermesUserGroups").HasData(new { HermesUserId = user.Id, GroupId = testGroup.Id });
                builder.Entity("HermesUserContacts").HasData(new { HermesUserId = user.Id, ContactId = testGroup.Id }, new { HermesUserId = testGroup.Id, ContactId = user.Id });
            }

            builder.Entity<HermesUser>().HasData(listUsers);

        }
    }
}