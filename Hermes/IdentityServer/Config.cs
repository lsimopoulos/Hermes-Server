﻿using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Hermes.IdentityServer
{
    /// <summary>
    /// Configuration class for Identity Server.
    /// </summary>
    public static class Config
    {
        /// <summary>
        ///     Get api resources.
        /// </summary>
        public static IEnumerable<ApiResource> GetApiResources()
        {

            return new List<ApiResource>
            {
                new()
                {
                    Name = "hermes",
                    ApiSecrets =
                    {
                        new Secret("magicsuperdupersecret".Sha256())
                    },
                    UserClaims =
                    {
                        JwtClaimTypes.Audience,
                        JwtClaimTypes.Issuer,
                        JwtClaimTypes.JwtId,
                        JwtClaimTypes.Name,
                        "extid"
                    },
                    Scopes =  new List<string>(){"hermes"}

                }
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

                    AllowedScopes = {"hermes"},
                    AccessTokenType = AccessTokenType.Jwt,
                },
                new()
                {
                    ClientId = "chat_web_client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("websuperdupersecret".Sha256())
                    },
                    AllowedScopes = {"hermes"},
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowedCorsOrigins = new[]{ "https://localhost:55555"  },

                }
            };
        }

        /// <summary>
        /// Get api scopes.
        /// </summary>
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ("hermes")
            };
        }
    }
}