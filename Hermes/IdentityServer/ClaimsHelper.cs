using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4.Test;

namespace Hermes.IdentityServer
{
    public class ClaimsHelper
    {
        /// <summary>
        /// Get custom claims
        /// </summary>
        /// <param name="user"></param>
        public IEnumerable<Claim> GetCustomClaims(TestUser user)
        {
            var claims = new List<Claim>
            {
                new ("name", user.Username)
            };
            
            return claims;
        }
    }
}
