using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dokterspunt.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dokterspunt
{
    public class Program
    {
        public static void Main(string[] args)
        {
             var host = CreateHostBuilder(args).Build();
            //klassieke overbodige initializer manier
            //using (var scope = host.Services.CreateScope())
            //{
            //    var service = scope.ServiceProvider;

            //    try
            //    {
            //        var context = service.GetRequiredService<DokterspuntContext>();
            //        DBInitializer.Initializer(context);
            //    }
            //    catch (Exception ex)
            //    {
            //        var logger = service.GetRequiredService<ILogger<Program>>();
            //        logger.LogError(ex, "An error Occurred while seeding the database");
            //    }

            //}
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
