using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using only.App.User.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace only.App.User.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController:Controller
    {
        [HttpPost]
        public async Task<JObject> Login(LoginUser loginUser, string returnUrl = null)
        {

            var result = new JObject();
            if (ModelState.IsValid)
            {

                var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
                var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
                //var tokenResponse = await tokenClient.RequestClientCredentialsAsync("App.Api");
                var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(loginUser.userName, loginUser.passWord, "App.Api");

                if (tokenResponse.IsError)
                {
                    result = new JObject { { "error", tokenResponse.Error } };
                    
                    return result;
                }

                result = new JObject { {"token",tokenResponse.AccessToken } };


                return result;


            }
            return result;

        }

    }
}
