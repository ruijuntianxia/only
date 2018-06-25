using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using only.IdentityServer.App.Data.DbContext;
using Microsoft.EntityFrameworkCore;
using only.IdentityServer.App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace only.IdentityServer.App
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; }
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var conn = Configuration.GetConnectionString("DefaultConnection");
            var migrationsAssmly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            const string customUrl = "https://192.168.1.180:5000";
            #region 添加对IdentiyServer4配置内容处理 By Liyouming 2017-11-29


            //因为添加了ApplicationUser验证，所以必须有这一步，不然dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration - c PersistedGrantDbContext - o Data / Migrations / IdentityServer / PersistedGrantDb   或失败
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(conn));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var builder = services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                // this adds the user 
                .AddAspNetIdentity<ApplicationUser>()
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => 
                        b.UseSqlServer(conn, sql => 
                            sql.MigrationsAssembly(migrationsAssmly));
                })
                .AddOperationalStore(options => 
                {
                    options.ConfigureDbContext = b => 
                        b.UseSqlServer(conn, sql => 
                            sql.MigrationsAssembly(migrationsAssmly));
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30;
                });

            //if (Environment.IsDevelopment())
            //{
            //    builder.AddDeveloperSigningCredential();
            //}
            //else
            //{
            //    throw new Exception("need to configure key material");
            //}

            
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
           // Initialization.InitializeDatabase(app);
            
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseHttpsRedirection();
            app.UseMvcWithDefaultRoute();
            app.UseMvc(routes => {
                
                routes.MapRoute(
                        name: "default",
                        template: "{controller=Identity}/{action=Register}");
            });
        }
    }
}
