using IdentityServer4.Test;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hermes.Models
{
    public class HermesUser : IdentityUser<Guid>
    {
        public HermesUser()
        {
            Id = Guid.NewGuid();
        }
        public bool IsGroup { get; set; }
        public virtual ICollection<HermesUser> Contacts { get; set; } = new HashSet<HermesUser>();
        public virtual ICollection<HermesUser> Groups { get; set; } = new HashSet<HermesUser>();
        public string Name { get; set; }
        [NotMapped]
        public string Password { get; set; }
    }
}
