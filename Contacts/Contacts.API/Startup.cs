using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using SampleOidc.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using SampleOidc.Contacts.API.Services;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Swagger;
using SampleOidc.Contacts.API.Filters;

namespace SampleOidc.Contacts.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            LoggerFactory = loggerFactory;
            CreateMaps();
        }

        private void CreateMaps()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<DTOs.AddContactDTO, Models.Contact>();
                cfg.CreateMap<DTOs.UpdateContactDTO, Models.Contact>();

                cfg.CreateMap<DTOs.AddAddressDTO, Models.Address>();
                cfg.CreateMap<DTOs.UpdateAddressDTO, Models.Address>();

                cfg.CreateMap<DTOs.AddEmailDTO, Models.Email>();
                cfg.CreateMap<DTOs.UpdateEmailDTO, Models.Email>();

                cfg.CreateMap<DTOs.AddPhoneDTO, Models.Phone>();
                cfg.CreateMap<DTOs.UpdatePhoneDTO, Models.Phone>();

            });
        }

        public IConfiguration Configuration { get; }
        public ILoggerFactory LoggerFactory { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {




            services.AddDbContext<ContactsDbContext>(options =>
             options.UseSqlServer(Configuration["ConnectionString"]));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IdentityService>();
            services.AddTransient<ContactsRepository>();

            ConfigureAuthService(services);

            services.AddCors(options =>
                        {
                            options.AddPolicy("CorsPolicy",
                                builder => builder.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials());
                        });

            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "Contacts HTTP API",
                    Version = "v1",
                    Description = "The Contacts Service HTTP API"
                });

                options.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "implicit",
                    AuthorizationUrl = $"{Configuration.GetValue<string>("IdentityUrl")}/connect/authorize",
                    TokenUrl = $"{Configuration.GetValue<string>("IdentityUrl")}/connect/token",
                    Scopes = new Dictionary<string, string>()
                    {
                        { "contacts", "Contacts API" }
                    }
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });

        }




        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseMvc();

            app.UseSwagger()
               .UseSwaggerUI(c =>
               {
                   c.SwaggerEndpoint($"{string.Empty}/swagger/v1/swagger.json", "Contacts.API V1");
                   c.ConfigureOAuth2("contactsswaggerui", "", "", "Contacts Swagger UI");
               });
        }



        private void ConfigureAuthService(IServiceCollection services)
        {
            // prevent from mapping "sub" claim to nameidentifier.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var identityUrl = Configuration.GetValue<string>("IdentityUrl");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = identityUrl;
                options.RequireHttpsMetadata = false;
                options.Audience = "contacts";
            });
        }
    }
}
