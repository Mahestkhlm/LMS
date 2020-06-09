using LMSLexicon20.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Data
{
    public class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services, string teacherPW)
        {
            var options = services.GetRequiredService<DbContextOptions<ApplicationDbContext>>();

            using (var _context = new ApplicationDbContext(options))
            {
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var roleNames = new[] { "Teacher", "Student" }; 

                //--------SKAPA ROLLER--------
                foreach (var name in roleNames)
                {
                    if (await roleManager.RoleExistsAsync(name)) continue;
                    var role = new IdentityRole { Name = name };
                    var result = await roleManager.CreateAsync(role);
                    if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
                }

                //--------SKAPA TEACHER1--------
                //ToDo remove in prod
                if (teacherPW is null) teacherPW = "abc123";
                var teacherEmail = "teacher1@lms.se";
                var teacherUser = await userManager.FindByEmailAsync(teacherEmail);
                if (teacherUser == null)
                {
                    var user = new User
                    {
                        UserName = teacherEmail,
                        FirstName = "Teacher1",
                        LastName = "1",
                        Email = teacherEmail,
                        RegDate = DateTime.Now
                    };

                //--------LÄGG TILL TEACHER1--------
                var addTeacherResult = await userManager.CreateAsync(user, teacherPW);
                if (!addTeacherResult.Succeeded) throw new Exception(string.Join("\n", addTeacherResult.Errors));
                }

                //--------GE ROLL TEACHER--------
                var teacher = await userManager.FindByNameAsync(teacherEmail);
                //var teacherRole = roleNames.FirstOrDefault(r => r == "Teacher");

                if (!await userManager.IsInRoleAsync(teacher, "Teacher"))
                {
                    var addToRoleResult = await userManager.AddToRoleAsync(teacher, "Teacher");
                    if (!addToRoleResult.Succeeded) throw new Exception(string.Join("\n", addToRoleResult.Errors));
                }

                var CourseId = _context.Courses.Where(c => c.Name == ".Net").Select(c => c.Id).FirstOrDefault();
                if (_context.Courses.Find(CourseId)?.Id is null)
                {
                    _context.Courses.Add(new Course { Name = ".Net", StartDate = new DateTime(2020, 6, 27, 20, 0, 0), Description = "I den här självstudien visas hur du skapar en .NET Core-app och ansluter den till SQL Database. När du är klar har du en .NET Core MVC-app som körs i App Service" });
                    _context.Courses.Add(new Course { Name = "Azure", StartDate = new DateTime(2020, 5, 27, 20, 0, 0), Description = "Automatically deploy and update a static web application and its API from a GitHub repository.\nIn this module, you will:\nChoose an existing web app project with either Angular,\nReact,\nSvelte or Vue\nCreate an API for the app with Azure Functions\nRun the application locally\nPublish the app and its API to Azure Static Web Apps" });
                    _context.SaveChanges();
                    CourseId = _context.Courses.Where(c => c.Name == ".Net").Select(c => c.Id).FirstOrDefault();
                }
                var ModuleId = _context.Modules.Where(m => m.Name == "Azure deploy").Select(m => m.Id).FirstOrDefault();
                if (_context.Modules.Find(ModuleId)?.Id is null)
                {
                    _context.Modules.Add(new Module { CourseId = CourseId, Name = "Azure deploy", StartDate = new DateTime(2020, 6, 27, 20, 0, 0), Description = "Deploy a website to Azure with Azure App Service" });
                    _context.Modules.Add(new Module { CourseId = CourseId, Name = "Azure Well", StartDate = new DateTime(2020, 6, 27, 20, 0, 0), Description = "Build great solutions with the Microsoft Azure Well - Architected Framework" });
                    _context.SaveChanges();
                    ModuleId = _context.Modules.Where(m => m.Name == "Azure deploy").Select(m => m.Id).FirstOrDefault();
                }

                var ActivityTypeId = _context.ActivityTypes.Where(a => a.Name == "e-learningpass").Select(m => m.Id).FirstOrDefault();
                if (_context.ActivityTypes.Find(ActivityTypeId)?.Id is null)
                {
                    _context.ActivityTypes.Add(new ActivityType { Name = "e-learningpass" });
                    _context.ActivityTypes.Add(new ActivityType { Name = "föreläsningar" });
                    _context.ActivityTypes.Add(new ActivityType { Name = "övningstillfällen" });
                    _context.ActivityTypes.Add(new ActivityType { Name = "annat" });
                    _context.SaveChanges();
                    ActivityTypeId = _context.ActivityTypes.Where(a => a.Name == "e-learningpass").Select(m => m.Id).FirstOrDefault();
                }

                var ActivityId = _context.Activities.Where(a => a.Name == "Environment").Select(m => m.Id).FirstOrDefault();
                if (_context.Activities.Find(ActivityId)?.Id is null)
                {
                    _context.Activities.Add(new Activity { ModuleId = ModuleId, ActivityTypeId = ActivityTypeId, Name = "Environment", StartDate = new DateTime(2020, 6, 27, 20, 0, 0), Description = "Prepare your development environment for Azure development" });
                    _context.Activities.Add(new Activity { ModuleId = ModuleId, ActivityTypeId = ActivityTypeId, Name = "App service", StartDate = new DateTime(2020, 5, 27, 20, 0, 0), Description = "Host a web application with Azure App service" });
                    _context.Activities.Add(new Activity { ModuleId = ModuleId, ActivityTypeId = ActivityTypeId, Name = "Web app platform", StartDate = new DateTime(2020, 5, 27, 20, 0, 0), Description = "Learn how to create a website through the hosted web app platform in Azure App Service" });
                    _context.SaveChanges();
                }


                //--------SKAPA STUDENT--------
                var studentPW = "abc123";
                var studentEmail = "student1@lms.se";
                var studentUser = await userManager.FindByEmailAsync(studentEmail);
                if (studentUser == null)
                {
                    var user = new User
                    {
                        UserName = studentEmail,
                        FirstName = "Student2",
                        LastName = "2",
                        Email = studentEmail,
                        RegDate = DateTime.Now
                    };

                    //--------LÄGG TILL STUDENT2--------
                    var addTeacherResult = await userManager.CreateAsync(user, studentPW);
                    if (!addTeacherResult.Succeeded) throw new Exception(string.Join("\n", addTeacherResult.Errors));
                }

                //--------GE ROLL STUDENT--------
                var student = await userManager.FindByNameAsync(studentEmail);

                if (!await userManager.IsInRoleAsync(student, "Student"))
                {
                    var addToRoleResult = await userManager.AddToRoleAsync(student, "Student");
                    if (!addToRoleResult.Succeeded) throw new Exception(string.Join("\n", addToRoleResult.Errors));
                }



            }

        }
    }
}
