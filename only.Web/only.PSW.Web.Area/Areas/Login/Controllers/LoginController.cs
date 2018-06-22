using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using only.PSW.Web.Area.Areas.Identity.Data;
using only.PSW.Web.Area.Areas.Identity.Models;
using only.PSW.Web.Area.Areas.Login.Models;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public LoginController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
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
                var tokenClient = new TokenClient(disco.TokenEndpoint, "or.client", "secret");
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
                //var content = await client.GetStringAsync("http://localhost:5001/identity");

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
