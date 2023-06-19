using Hermes.Models;
using System.Collections.Generic;
using System.Security.Claims;

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
             new Claim("id",user.Id.ToString())
            };

            return claims;
        }
    }
}
