using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using SampleOidc.Identity.API.Configuration;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleOidc.Identity.API.Data
{
    public  class ConfigurationDbContextSeed
    {
        public async Task SeedAsync(ConfigurationDbContext context,IConfiguration configuration)
        {
           
            //callbacks urls from config:
            var clientUrls = new Dictionary<string, string>();
            
            clientUrls.Add("Spa", configuration.GetValue<string>("SpaClient"));            
            
            if (!context.Clients.Any())
            {
                foreach (var client in Config.GetClients(clientUrls))
                {
                    await context.Clients.AddAsync(client.ToEntity());
                }
                await context.SaveChangesAsync();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.GetResources())
                {
                    await context.IdentityResources.AddAsync(resource.ToEntity());
                }
                await context.SaveChangesAsync();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var api in Config.GetApis())
                {
                    await context.ApiResources.AddAsync(api.ToEntity());
                }

                await context.SaveChangesAsync();
            }
        }
    }
}