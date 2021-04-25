using System;
using System.Linq;
using System.Threading.Tasks;
using Hermes.Classes;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace Hermes.IdentityServer
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly ClaimsHelper _claimsHelper;
        private readonly UsersManagers _usersManagers;
        private readonly CryptoHelper _cryptoHelper;

        public ResourceOwnerPasswordValidator(ClaimsHelper claimsHelper, UsersManagers usersManagers,CryptoHelper cryptoHelper)
        {
            _claimsHelper = claimsHelper;
            _usersManagers = usersManagers;
            _cryptoHelper = cryptoHelper;
        }
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var currentUser = _usersManagers.GetUsers().FirstOrDefault(x =>
                x.Username.ToLowerInvariant().Equals(context.UserName.ToLowerInvariant()));
            if (currentUser != null )
            {
                var decryptedPassword = _cryptoHelper.Decrypt(currentUser.Password);
                if (decryptedPassword.Equals(context.Password,StringComparison.InvariantCulture)) 
                    context.Result = new GrantValidationResult(
                    currentUser.SubjectId, OidcConstants.AuthenticationMethods.Password, _claimsHelper.GetCustomClaims(currentUser));
                return Task.FromResult(context);

            }

            context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient,
                "Wrong username/password");

            return Task.FromResult(context);
        }


    }

}
