using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using only.PSW.Web.Area.Areas.Identity.Data;
using only.PSW.Web.Area.Areas.Identity.Models;
using only.PSW.Web.Area.Areas.Login.Models;
using only.PSW.Web.Area.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace only.PSW.Web.Area.Areas.Login.Controllers
{
    [Area("Login")]
    public class LoginController:Controller
    {

        private readonly ApplicationDbContext _context;
        public LoginController(ApplicationDbContext context)
        {
            _context = context;

        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUser loginUser,string returnUrl = null)
        {

            if(ModelState.IsValid)
            {

                var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
                var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
                //var tokenResponse = await tokenClient.RequestClientCredentialsAsync("App.Api");
                var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(loginUser.userName, loginUser.passWord, "App.Api");

                if (tokenResponse.IsError)
                {
                    Console.WriteLine(tokenResponse.Error);
                    ViewData["Error"] = tokenResponse.Error;
                    return View();
                }
                var client = new HttpClient();
                client.SetBearerToken(tokenResponse.AccessToken);
                var sf = client.BaseAddress;

                //var content = await client.GetStringAsync("http://localhost:5001/identity");
                var claims = new List<Claim>
                {
                    new Claim("UserName",loginUser.userName),
                    new Claim("Api","App.Api")
                };
                var claimsIdentity = new ClaimsIdentity(
                    claims, CustomerAuthorizeAttribute.CustomerAuthenticationScheme);
                await HttpContext.SignInAsync(CustomerAuthorizeAttribute.CustomerAuthenticationScheme,new ClaimsPrincipal(claimsIdentity));
                //ViewBag.Json = JArray.Parse(content).ToString();
                ViewData["ReturnUrl"] = returnUrl;
                return  RedirectToAction("Index", "Home", new { area = "Admin" });


            }
            return View();

        }

       




        public async Task<IActionResult> CallApiUsingClientCredentials()
        {
            var tokenClient = new TokenClient("http://localhost:5000/connect/token", "mvc", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("App.Api");

            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            var content = await client.GetStringAsync("http://localhost:5001/identity");

            ViewBag.Json = JArray.Parse(content).ToString();
            return View("json");
        }

        public async Task<IActionResult> CallApiUsingUserAccessToken()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.SetBearerToken(accessToken);
            var content = await client.GetStringAsync("http://localhost:5001/identity");

            ViewBag.Json = JArray.Parse(content).ToString();
            return View("json");
        }
    }
}
