using System.Collections.Generic;
using System.Linq;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Hermes.IdentityServer
{
    public static class Config
    {
        /// <summary>
        /// Configuration class for Identity Server.
        /// </summary>
        private static readonly List<TestUser> RegisteredUsers = new()
        {
            new TestUser
            {
                SubjectId = "1",
                Username = "leo",
                Password = "password"
            },
            new TestUser
            {
                SubjectId = "2",
                Username = "test",
                Password = "pass"
            }
        };

        /// <summary>
        ///     Get api resources.
        /// </summary>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new("hermes", "Hermes Chat", GetApiScopes().Select(x => x.Name).ToList())
            };
        }

        /// <summary>
        ///     Get clients.
        /// </summary>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new()
                {
                    ClientId = "chat_console_client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("superdupersecret".Sha256())
                    },
                    AllowedScopes = {"hermes"}
                },
                new()
                {
                    ClientId = "chat_web_client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("websuperdupersecret".Sha256())
                    },
                    AllowedScopes = {"hermes"}
                }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ("hermes")
            };
        }


        /// <summary>
        ///     Get users.
        /// </summary>
        public static List<TestUser> GetUsers()
        {
            return RegisteredUsers;
        }

        public static void AddUser(TestUser user)
        {
            user.SubjectId = (RegisteredUsers.Count + 1).ToString();
            RegisteredUsers.Add(user);
        }
    }
}