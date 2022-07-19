using System.Collections.Generic;
using System.Security.Claims;
using Hermes.Models;
using IdentityServer4.Test;

namespace Hermes.IdentityServer
{
    public class ClaimsHelper
    {
        /// <summary>
        /// Get custom claims
        /// </summary>
        /// <param name="user"></param>
        public IEnumerable<Claim> GetCustomClaims(HermesUser user)
        {
            var claims = new List<Claim>
            {
             new Claim("extid",user.ExternalId.ToString())
            };

            return claims;
        }
    }
}
