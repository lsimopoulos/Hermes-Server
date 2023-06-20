using IdentityServer4.Models;
using IdentityServer4.Stores;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes.IdentityServer
{
    public class CustomClientStore : IClientStore
    {
        public Task<Client> FindClientByIdAsync(string clientId)
        {
            return Task.FromResult(Config.GetClients().Where(x => x.ClientId == clientId).FirstOrDefault());
        }
    }
}
