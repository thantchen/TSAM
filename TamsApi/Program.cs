using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TamsApi.Data;
using TamsApi.Data.Seeds;

namespace TamsApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await Seed(host);
            host.Run();
        }

        public static async Task Seed(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetService<ILogger<Program>>();
            try
            {
               var users = new Users(
                    services.GetService<IUserRepository>(),
                    services.GetService<IOptions<SeedOptions>>(),
                    logger
                );
                await users.Seed();
            }
            catch (AggregateException e)
            {
                logger.LogWarning(e, "Failed to seed application.");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
