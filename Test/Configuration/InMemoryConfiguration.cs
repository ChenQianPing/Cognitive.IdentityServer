using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Configuration
{
    public class InMemoryConfiguration
    {
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// Define which APIs will use this IdentityServer
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource("clientservice", "CAS Client Service"),
                new ApiResource("productservice", "CAS Product Service"),
                new ApiResource("agentservice", "CAS Agent Service")
            };
        }

        /// <summary>
        /// Define which Apps will use thie IdentityServer
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "client.api.service",
                    ClientSecrets = new [] { new Secret("clientsecret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new [] { "clientservice" }
                },
                new Client
                {
                    ClientId = "product.api.service",
                    ClientSecrets = new [] { new Secret("productsecret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new [] { "clientservice", "productservice" }
                },
                new Client
                {
                    ClientId = "agent.api.service",
                    ClientSecrets = new [] { new Secret("agentsecret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new [] { "agentservice", "clientservice", "productservice" }
                }
            };
        }

        /// <summary>
        /// Define which uses will use this IdentityServer
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TestUser> GetUsers()
        {
            return new[]
            {
                new TestUser
                {
                    SubjectId = "10001",
                    Username = "bobby@hotmail.com",
                    Password = "bobbypassword"
                },
                new TestUser
                {
                    SubjectId = "10002",
                    Username = "andy@hotmail.com",
                    Password = "andypassword"
                },
                new TestUser
                {
                    SubjectId = "10003",
                    Username = "leo@hotmail.com",
                    Password = "leopassword"
                }
            };
        }
    }
}

/*
 * 为了要把IdentityServer注册到容器中，需要对其进行配置，而这个配置中要包含三个信息：
 　 （1）哪些API可以使用这个AuthorizationServer
　　（2）哪些Client可以使用这个AuthorizationServer
　　（3）哪些User可以被这个AuthrizationServer识别并授权

　　这里为了快速演示，我们写一个基于内存的静态类来快速填充上面这些信息
  （实际中，可以持久化在数据库中通过EF等ORM获取，也可以通过Redis获取）


    POST http://localhost:12394/connect/token
Body form-data
client_id：product.api.service
client_secret：productsecret
grant_type：password
username：bobby@hotmail.com
password：bobbypassword


Authorization Bearer access_token

{
    "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6IjY3NzM1ZDgwNTc5YzI5MGU2ZjU3ZDQyM2RmNTMzYjNjIiwidHlwIjoiSldUIn0.eyJuYmYiOjE1MzQxMjY2NDgsImV4cCI6MTUzNDEzMDI0OCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDoxMjM5NCIsImF1ZCI6WyJodHRwOi8vbG9jYWxob3N0OjEyMzk0L3Jlc291cmNlcyIsImNsaWVudHNlcnZpY2UiLCJwcm9kdWN0c2VydmljZSJdLCJjbGllbnRfaWQiOiJwcm9kdWN0LmFwaS5zZXJ2aWNlIiwic3ViIjoiMTAwMDEiLCJhdXRoX3RpbWUiOjE1MzQxMjY2NDgsImlkcCI6ImxvY2FsIiwic2NvcGUiOlsiY2xpZW50c2VydmljZSIsInByb2R1Y3RzZXJ2aWNlIl0sImFtciI6WyJwd2QiXX0.K9u-b5m1rb5ifZuLFpJYuHOfFcU8ib5HFplaDkj0eI2SVIKM_cKeCfo3tmBURoUs_dKY43TuwRAXWOtLOIBoLDkXVFyGKtMO6rRRyaommNHKlySI3xQL3jkUbgv5KxS7VuJ8aE--KElxVhyR-iX_GVRT6fuNtdXcgXvcwiGvdCBr2tjCoMMlH_udHW6vb4ToLd6X_lBiwtG_JY04pB9PICLiZHbjGnUQvOa6oG5dMzL3wJeQMgjGqYX2Aq0QUXlzVDxy-e93imI29KvYRVhS88U39W-8IPRSWd-cuBN7fxLDa8f5Ue2SPlN6cxHSK0f_L9u8gRk3jEEYZ8z7-b_1pQ", 
    "expires_in": 3600, 
    "token_type": "Bearer"
}


{
    "error": "invalid_grant", 
    "error_description": "invalid_username_or_password"
}

 */
