using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SampleOidc.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SampleOidc.Contacts.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
            .MigrateDbContext<ContactsDbContext>((_, __) => { })
            .Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var webconfig = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("hosting.json")
            .Build();

            return WebHost.CreateDefaultBuilder(args)
                 .UseStartup<Startup>()
                 .UseConfiguration(webconfig)
                 .ConfigureAppConfiguration((builderContext, config) =>
                    {
                        config.AddEnvironmentVariables();
                        
                        config.AddJsonFile("appsettings.json");
                    })
                    .ConfigureLogging((hostingContext, builder) =>
                    {
                        builder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                        builder.AddConsole();
                        builder.AddDebug();
                    })
                 .Build();
        }
    }

    public static class IWebHostExtensions
    {
        public static IWebHost MigrateDbContext<TContext>(this IWebHost webHost, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var logger = services.GetRequiredService<ILogger<TContext>>();

                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation($"Migrating database associated with context {typeof(TContext).Name}");

                    context.Database
                        .Migrate();

                    seeder(context, services);

                    logger.LogInformation($"Migrated database associated with context {typeof(TContext).Name}");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred while migrating the database used on context {typeof(TContext).Name}");
                }
            }

            return webHost;
        }
    }
}
