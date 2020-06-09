using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services, string adminPW)
        {
            var options = services.GetRequiredService<DbContextOptions<ApplicationDbContext>>();

            using (var context = new ApplicationDbContext(options))
            {
                if (context.Courses.Find(1)?.Id is null) context.Courses.Add(new GymClass { Id = 1, Name = ".Net", StartDate = new DateTime(2020, 6, 27, 20, 0, 0), Description = "I den här självstudien visas hur du skapar en .NET Core-app och ansluter den till SQL Database. När du är klar har du en .NET Core MVC-app som körs i App Service" });
                if (context.Courses.Find(2)?.Id is null) context.Courses.Add(new GymClass { Id = 2, Name = "Azure", StartDate = new DateTime(2020, 5, 27, 20, 0, 0), Description = "Automatically deploy and update a static web application and its API from a GitHub repository.\nIn this module, you will:\nChoose an existing web app project with either Angular,\nReact,\nSvelte or Vue\nCreate an API for the app with Azure Functions\nRun the application locally\nPublish the app and its API to Azure Static Web Apps" });

// Users
// Module
// Activities
            }
        }
    }
}

