using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SampleOidc.Identity.API.Data;
using SampleOidc.Identity.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.EntityFramework;
using IdentityServer4.Services;
using SampleOidc.Identity.API.Services;
using SampleOidc.Identity.API.Certificate;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace SampleOidc.Identity.API
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }



        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ApplicationDBContext>(options =>
             options.UseSqlServer(Configuration["ConnectionString"]));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders();

            services.Configure<AppSettings>(Configuration);

            services.AddMvc();

            // Cluster
            // Health

            services.AddTransient<ILoginService<ApplicationUser>, LoginService>();
            services.AddTransient<IRedirectService, RedirectService>();

            var connectionString = Configuration["ConnectionString"];
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer(x => x.IssuerUri = "null")
                .AddSigningCredential(Certificate.Certificate.Get())
                .AddAspNetIdentity<ApplicationUser>()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString, opts =>
                        opts.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                 {
                     options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString, opts =>
                          opts.MigrationsAssembly(migrationsAssembly));
                 })
                .Services.AddTransient<IProfileService, ProfileService>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                loggerFactory.CreateLogger("init").LogDebug($"Using PATH BASE '{pathBase}'");
                app.UsePathBase(pathBase);
            }

            app.UseStaticFiles();

            // Make work identity server redirections in Edge and lastest versions of browers. WARN: Not valid in a production environment.
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Content-Security-Policy", "script-src 'unsafe-inline'");
                await next();
            });

            app.UseAuthentication();

            app.UseIdentityServer();

            app.UseMvc((routes) =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Index}/{id?}");
            });
        }
    }
}
