using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using only.IdentityServer.App.Data.DbContext;
using Microsoft.EntityFrameworkCore;
using only.IdentityServer.App.Models;
using Microsoft.AspNetCore.Identity;
using only.IdentityServer.App.Common;

namespace only.IdentityServer.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var conn = Configuration.GetConnectionString("DefaultConnection");
            var migrationsAssmly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            //因为添加了ApplicationUser验证，所以必须有这一步，不然dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration - c PersistedGrantDbContext - o Data / Migrations / IdentityServer / PersistedGrantDb   或失败
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(conn));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                // this adds the user 
                .AddAspNetIdentity<ApplicationUser>()
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder => 
                        builder.UseSqlServer(conn, sql => 
                            sql.MigrationsAssembly(migrationsAssmly));
                })
                .AddOperationalStore(options => 
                {
                    options.ConfigureDbContext = builder => 
                        builder.UseSqlServer(conn, sql => 
                            sql.MigrationsAssembly(migrationsAssmly));
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30;
                });
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
