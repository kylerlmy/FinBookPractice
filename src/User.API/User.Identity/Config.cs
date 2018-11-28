using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Identity
{
    public class Config
    {
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "iphone",
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    RefreshTokenExpiration=TokenExpiration.Sliding,//滑动
                    AllowOfflineAccess=true,
                    RequireClientSecret=false,
                    AllowedGrantTypes = new List<string>{ "sms_auth_code"},
                    AlwaysIncludeUserClaimsInIdToken=true,
                    AllowedScopes = new List<string>{
                        "gateway_api",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                    }
                },
                 new Client
                {
                    ClientId = "android",
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    RefreshTokenExpiration=TokenExpiration.Sliding,//滑动
                    AllowOfflineAccess=true,
                    RequireClientSecret=false,
                    AllowedGrantTypes = new List<string>{ "sms_auth_code"},
                    AlwaysIncludeUserClaimsInIdToken=true,//将Claims加入到Token里面去
                    AllowedScopes = new List<string>{
                        "gateway_api",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                    }
                },
            };
        }

        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("gateway_api", "API Application"),
                //new ApiResource("gateway_api","gateway service"),
                new ApiResource("user_api","user service"),
                new ApiResource("project_api","project service"),
                new ApiResource("contact_api","contact service")
               // new ApiResource("user_api", "user service")
            };
        }

        // clients want to access resources (aka scopes)
    }
}
