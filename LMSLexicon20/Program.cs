using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMSLexicon20.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LMSLexicon20
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using(var scope= host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
                var config = services.GetRequiredService<IConfiguration>();

                //ANV�ND F�R ERAT L�SEN
                //(i cmnd line, st� i solution-folder. Ex: C:\Users\josefin\source\repos\LMSLexicon20\LMSLexicon20) :
                //dotnet user-secrets set "TeacherPW" "ert l�senord"  
                var teacherPW = config["TeacherPW"];

                try
                {
                    SeedData.InitializeAsync(services, teacherPW).Wait();
                }
                catch (Exception ex )
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex.Message, "Seed Failed");
                }
            }
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
