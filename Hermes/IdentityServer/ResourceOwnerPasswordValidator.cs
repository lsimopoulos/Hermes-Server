using Hermes.Models;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Hermes.IdentityServer
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly ClaimsHelper _claimsHelper;
        //private readonly CryptoHelper _cryptoHelper;
        private readonly SignInManager<HermesUser> _signInManager;
        private readonly UserManager<HermesUser> _userManager;

        public ResourceOwnerPasswordValidator(ClaimsHelper claimsHelper, SignInManager<HermesUser> signInManager, UserManager<HermesUser> userManager)
        {
            _claimsHelper = claimsHelper;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _userManager.FindByNameAsync(context.UserName);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, context.Password, true, true);
                if (result.Succeeded)
                {

                    context.Result = new GrantValidationResult(
                    user.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
                    return;
                }
            }
            context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient,
                "Wrong username/password");

            await Task.FromResult(context);
        }


    }

}
