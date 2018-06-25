using IdentityServer4;
using IdentityServer4.Models;
using only.IdentityServer.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace only.IdentityServer.App.Common
{
    public class ClientMethod
    {

        public static IEnumerable<Client> GetClients(ClientViewModel model)
        {
            var granttypes = GrantTypes.Implicit;
            switch (model.AllowedGrantTypes)
            {
                case "Implicit":
                    granttypes = GrantTypes.Implicit;
                    break;
                case "ImplicitAndClientCredentials":
                    granttypes = GrantTypes.ImplicitAndClientCredentials;
                    break;
                case "Code":
                    granttypes = GrantTypes.Code;
                    break;
                case "CodeAndClientCredentials":
                    granttypes = GrantTypes.CodeAndClientCredentials;
                    break;
                case "Hybrid":
                    granttypes = GrantTypes.Hybrid;
                    break;
                case "HybridAndClientCredentials":
                    granttypes = GrantTypes.HybridAndClientCredentials;
                    break;
                case "ClientCredentials":
                    granttypes = GrantTypes.ClientCredentials;
                    break;
                case "ResourceOwnerPassword":
                    granttypes = GrantTypes.ResourceOwnerPassword;
                    break;
                case "ResourceOwnerPasswordAndClientCredentials":
                    granttypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials;
                    break;
                default:
                    break;

            }


            // client credentials client
            return new List<Client>
            {
                // JavaScript Client
                new Client
                {
                    ClientId = model.ClientId,
                    ClientName = model.ClientName,
                    ClientSecrets={ new Secret(model.ClientSecrets.Sha256())},
                    AllowedGrantTypes = granttypes,
                    AllowAccessTokensViaBrowser = model.AllowAccessTokensViaBrowser,

                    RedirectUris ={model.RedirectUris==null?"": model.RedirectUris},// { "http://localhost:5003/callback.html" },
                    PostLogoutRedirectUris ={ model.PostLogoutRedirectUris == null ? "" : model.PostLogoutRedirectUris }, //{ "http://localhost:5003/index.html" },
                    AllowedCorsOrigins = { model.AllowedCorsOrigins == null ? "" : model.AllowedCorsOrigins },//{ "http://localhost:5003" },


                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                        
                    },
                    AllowOfflineAccess = model.AllowOfflineAccess
                }
            };
        }


        public static IEnumerable<ApiResource> GetApiResources(ApiResourceViewModel model)
        {
            return new List<ApiResource>
            {
                new ApiResource(model.Name,model.DisplayName)
            };
        }
    }
}
