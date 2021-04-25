using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace Hermes.IdentityServer
{
    public class CustomClientStore : IClientStore
    {
        public Task<Client> FindClientByIdAsync(string clientId)
        {
           return Task.FromResult(Config.GetClients().FirstOrDefault(x => x.ClientId == clientId));
        }
    }
}
