using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using only.IdentityServer.App.Data.DbContext;
using only.IdentityServer.App.Models;

namespace only.IdentityServer.App.Controllers
{

    public class IdentityController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public IdentityController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Database.Migrate();


                var user = _userManager.FindByNameAsync(model.UserName).Result;
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = model.UserName,
                        Email = model.UserMail
                    };
                    var result = _userManager.CreateAsync(user, model.PassWord).Result;
                    if (!result.Succeeded)
                    {
                        ViewData["Error"] = result.Errors.First().Description;
                        return View();

                    }

                    result = _userManager.AddClaimsAsync(user, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Alice Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Alice"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                            new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                        }).Result;
                    if (!result.Succeeded)
                    {
                        ViewData["Error"] = result.Errors.First().Description;
                        return View();
                    }

                    ViewData["Error"] = "新用户添加成功！";

                }
                else
                {
                    ViewData["Error"] = "新用户添加失败！";
                }
                return View();

            }
            ViewData["Error"] = "请填写完全！";
            return View();
            
        }
    }
}