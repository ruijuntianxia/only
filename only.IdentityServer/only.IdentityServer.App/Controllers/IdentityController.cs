using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using only.IdentityServer.App.Common;
using only.IdentityServer.App.Data.DbContext;
using only.IdentityServer.App.Models;

namespace only.IdentityServer.App.Controllers
{

    public class IdentityController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly ConfigurationDbContext _contextConfig;
        private readonly PersistedGrantDbContext _contextPers;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public IdentityController(ApplicationDbContext context, ConfigurationDbContext contextConfig,PersistedGrantDbContext contextPers,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _contextConfig = contextConfig;
            _contextPers = contextPers;
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
                            new Claim(JwtClaimTypes.Name, model.FamilyName+" "+model.UserName),
                            new Claim(JwtClaimTypes.GivenName, model.UserName),
                            new Claim(JwtClaimTypes.FamilyName, model.FamilyName),
                            new Claim(JwtClaimTypes.Email,model.UserMail),
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

        public IActionResult Client()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Client(ClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                _contextConfig.Database.Migrate();
                var decide = _contextConfig.Clients.Count(m=>m.ClientId==model.ClientId);
                if (decide == 0)
                {
                    foreach (var client in ClientMethod.GetClients(model))
                    {
                        _contextConfig.Clients.Add(client.ToEntity());
                    }
                    try
                    {
                        _contextConfig.SaveChanges();
                        ViewData["Error"] = "客户端注册成功";
                    }
                    catch (Exception ex)
                    {
                        ViewData["Error"] = "客户端注册失败!错误:" + ex.ToString();

                    }

                }
                else
                {
                    ViewData["Error"] = "客户端已注册！";
                }
            }
           
            
            return View();
        }


        public IActionResult InsertApi()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InsertApi(ApiResourceViewModel model)
        {
            if (ModelState.IsValid)
            {
                 _contextConfig.Database.Migrate();

                var api = _contextConfig.ApiResources.Count(m => m.Name == model.Name);


                if (api == 0)
                {
                    foreach (var resource in ClientMethod.GetApiResources(model))
                    {
                        _contextConfig.ApiResources.Add(resource.ToEntity());
                    }
                    try
                    {
                        _contextConfig.SaveChanges();
                        ViewData["Error"] = "服务端注册成功";
                    }
                    catch (Exception ex)
                    {

                        ViewData["Error"] = "服务端注册失败!错误:" + ex.ToString();
                    }

                }
                else;
                {
                    ViewData["Error"] = "服务已存在！";
                }
            }
           
            return View();
        }


        public static void InitializeDatabase(IApplicationBuilder _builder)
        {
            using (var serviceScope = _builder.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}