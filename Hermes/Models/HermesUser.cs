using IdentityServer4.Test;
using System;
using System.Collections.Generic;

namespace Hermes.Models
{
    public class HermesUser : TestUser
    {
        public HermesUser()
        {
            ExternalId = Guid.NewGuid();
            SubjectId = Guid.NewGuid().ToString();
        }

        public HermesUser(HermesUser user)
        {
            ExternalId = user.ExternalId;
            SubjectId = user.SubjectId;
            Username = user.Username;
            Name = user.Name+ " (me)";
        }
        public Guid ExternalId { get; }
        public ICollection<HermesUser> Contacts { get; set; } = new HashSet<HermesUser>();
        public ICollection<Group> Groups { get; set; } = new HashSet<Group>();
        public string Name { get; set; }
    }
}
