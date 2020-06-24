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

                var ActivityTypeId = _context.ActivityTypes.Where(a => a.Name == "e-learningpass").Select(m => m.Id).FirstOrDefault();
                if (ActivityTypeId > 0)
                {
                    var model = await _context.ActivityTypes.FindAsync(ActivityTypeId);
                    model.Name = "E-learningpass";
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                ActivityTypeId = _context.ActivityTypes.Where(a => a.Name == "föreläsningar").Select(m => m.Id).FirstOrDefault();
                if (ActivityTypeId > 0)
                {
                    var model = await _context.ActivityTypes.FindAsync(ActivityTypeId);
                    model.Name = "Föreläsning";
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                ActivityTypeId = _context.ActivityTypes.Where(a => a.Name == "övningstillfällen").Select(m => m.Id).FirstOrDefault();
                if (ActivityTypeId > 0)
                {
                    var model = await _context.ActivityTypes.FindAsync(ActivityTypeId);
                    model.Name = "Övningstillfälle";
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                ActivityTypeId = _context.ActivityTypes.Where(a => a.Name == "inlämmningsuppgift").Select(m => m.Id).FirstOrDefault();
                if (ActivityTypeId > 0)
                {
                    var model = await _context.ActivityTypes.FindAsync(ActivityTypeId);
                    model.Name = "Inlämmningsuppgift";
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                ActivityTypeId = _context.ActivityTypes.Where(a => a.Name == "annat").Select(m => m.Id).FirstOrDefault();
                if (ActivityTypeId > 0)
                {
                    var model = await _context.ActivityTypes.FindAsync(ActivityTypeId);
                    model.Name = "Annat";
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }


                ActivityTypeId = _context.ActivityTypes.Where(a => a.Name == "E-learningpass").Select(m => m.Id).FirstOrDefault();
                if (_context.ActivityTypes.Find(ActivityTypeId)?.Id is null)
                {
                    _context.ActivityTypes.Add(new ActivityType { Name = "E-learningpass" });
                    _context.ActivityTypes.Add(new ActivityType { Name = "Föreläsningar" });
                    _context.ActivityTypes.Add(new ActivityType { Name = "Övningstillfällen" });
                    _context.ActivityTypes.Add(new ActivityType { Name = "Annat" });
                    _context.SaveChanges();
                    ActivityTypeId = _context.ActivityTypes.Where(a => a.Name == "E-learningpass").Select(m => m.Id).FirstOrDefault();
                }

                ActivityTypeId = _context.ActivityTypes.Where(a => a.Name == "Inlämmningsuppgift").Select(m => m.Id).FirstOrDefault();
                if (_context.ActivityTypes.Find(ActivityTypeId)?.Id is null)
                {
                    _context.ActivityTypes.Add(new ActivityType { Name = "Inlämmningsuppgift", RequireDocument = true });
                    _context.SaveChanges();
                }

                //// DELETE in reverse order
                //var ActivityId = _context.Activities.Where(a => a.Name == "Environment").Select(m => m.Id).FirstOrDefault();
                //if (ActivityId>0) { _context.Activities.Remove(_context.Activities.Find(ActivityId)); _context.SaveChanges(); }
                //ActivityId = _context.Activities.Where(a => a.Name == "App service").Select(m => m.Id).FirstOrDefault();
                //if (ActivityId > 0) { _context.Activities.Remove(_context.Activities.Find(ActivityId)); _context.SaveChanges();}
                //ActivityId = _context.Activities.Where(a => a.Name == "Web app platform").Select(m => m.Id).FirstOrDefault();
                //if (ActivityId > 0) {_context.Activities.Remove(_context.Activities.Find(ActivityId)); _context.SaveChanges();}
                //ActivityId = _context.Activities.Where(a => a.Name == "Web app sample").Select(m => m.Id).FirstOrDefault();
                //if (ActivityId > 0) {_context.Activities.Remove(_context.Activities.Find(ActivityId)); _context.SaveChanges();}

                //var ModuleId = _context.Modules.Where(m => m.Name == "Azure deploy").Select(m => m.Id).FirstOrDefault();
                //if (ModuleId > 0) { _context.Modules.Remove(_context.Modules.Find(ModuleId)); _context.SaveChanges(); }
                //ModuleId = _context.Modules.Where(m => m.Name == "Azure Well").Select(m => m.Id).FirstOrDefault();
                //if (ModuleId > 0) { _context.Modules.Remove(_context.Modules.Find(ModuleId)); _context.SaveChanges(); }

                //var CourseId = _context.Courses.Where(c => c.Name == ".Net").Select(c => c.Id).FirstOrDefault();
                //if (CourseId > 0) { _context.Courses.Remove(_context.Courses.Find(CourseId)); _context.SaveChanges(); }


                var CourseId = _context.Courses.Where(c => c.Name == ".Net").Select(c => c.Id).FirstOrDefault();
                if (_context.Courses.Find(CourseId)?.Id is null)
                {
                    _context.Courses.Add(new Course { Name = ".Net", 
                        StartDate = new DateTime(2020, 6, 27, 0, 0, 0),
                        EndDate = new DateTime(2020, 8, 27, 0, 0, 0),
                        Description = "I den här självstudien visas hur du skapar en .NET Core-app och ansluter den till SQL Database. När du är klar har du en .NET Core MVC-app som körs i App Service" });
                    _context.Courses.Add(new Course { Name = "Azure", StartDate = new DateTime(2020, 5, 27, 0, 0, 0), Description = "Automatically deploy and update a static web application and its API from a GitHub repository.\nIn this module, you will:\nChoose an existing web app project with either Angular,\nReact,\nSvelte or Vue\nCreate an API for the app with Azure Functions\nRun the application locally\nPublish the app and its API to Azure Static Web Apps" });
                    _context.SaveChanges();
                }

                CourseId = _context.Courses.Where(a => a.Name == ".Net").Select(c => c.Id).FirstOrDefault();
                if (CourseId > 0)
                {
                    var model = await _context.Courses.FindAsync(CourseId);
                    model.EndDate = new DateTime(2020, 8, 27, 0, 0, 0);
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }


                var ModuleId = _context.Modules.Where(m => m.Name == "Azure deploy").Select(m => m.Id).FirstOrDefault();
                if (_context.Modules.Find(ModuleId)?.Id is null)
                {
                    CourseId = _context.Courses.Where(c => c.Name == ".Net").Select(c => c.Id).FirstOrDefault();
                    _context.Modules.Add(new Module { CourseId = CourseId, Name = "Azure deploy", 
                        StartDate = new DateTime(2020, 6, 15, 0, 0, 0),
                        EndDate = new DateTime(2020, 7, 27, 0, 0, 0),
                        Description = "Deploy a website to Azure with Azure App Service" });
                    _context.Modules.Add(new Module { CourseId = CourseId, Name = "Azure Well", 
                        StartDate = new DateTime(2020, 7, 27, 0, 0, 0),
                        EndDate = new DateTime(2020, 8, 29, 0, 0, 0),
                        Description = "Build great solutions with the Microsoft Azure Well - Architected Framework" });
                    _context.SaveChanges();
                }


                var ActivityId = _context.Activities.Where(a => a.Name == "Environment").Select(m => m.Id).FirstOrDefault();
                if (_context.Activities.Find(ActivityId)?.Id is null)
                {
                    ModuleId = _context.Modules.Where(m => m.Name == "Azure deploy").Select(m => m.Id).FirstOrDefault();
                    _context.Activities.Add(new Activity { ModuleId = ModuleId, ActivityTypeId = ActivityTypeId, Name = "Environment", 
                        StartDate = new DateTime(2020, 6, 15, 0, 0, 0),
                        EndDate = new DateTime(2020, 7, 7, 0, 0, 0),
                        Description = "Prepare your development environment for Azure development" });
                    _context.SaveChanges();

                    _context.Activities.Add(new Activity { ModuleId = ModuleId, ActivityTypeId = ActivityTypeId, Name = "App service", 
                        StartDate = new DateTime(2020, 7, 7, 0, 0, 0), 
                        EndDate = new DateTime(2020, 7, 17, 0, 0, 0),
                        Description = "Host a web application with Azure App service"
                    });
                    _context.SaveChanges();
                    _context.Activities.Add(new Activity { ModuleId = ModuleId, ActivityTypeId = ActivityTypeId, Name = "Web app platform", 
                        StartDate = new DateTime(2020, 7, 17, 0, 0, 0),
                        EndDate = new DateTime(2020, 8, 7, 0, 0, 0),
                        Description = "Learn how to create a website through the hosted web app platform in Azure App Service"
                    });

                    ActivityTypeId = _context.ActivityTypes.Where(a => a.Name == "E-learningpass").Select(m => m.Id).FirstOrDefault();
                    _context.Activities.Add(new Activity
                    {
                        ModuleId = ModuleId,
                        ActivityTypeId = ActivityTypeId,
                        Name = "Web app sample",
                        StartDate = new DateTime(2020, 7, 27, 0, 0, 0),
                        EndDate = new DateTime(2020, 8, 7, 0, 0, 0),
                        HasDeadline = true,
                        Description = "Create a simple website and deploy with Azure App Service"
                    }); ;
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
                        FirstName = "Student1",
                        LastName = "1",
                        Email = studentEmail,
                        RegDate = DateTime.Now,
                        CourseId= CourseId

                    };

                    //--------LÄGG TILL STUDENT1--------
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



                //--------SKAPA STUDENT2--------
                //var studentPW = "abc123";
                studentEmail = "student2@lms.se";
                studentUser = await userManager.FindByEmailAsync(studentEmail);
                if (studentUser == null)
                {
                    var user = new User
                    {
                        UserName = studentEmail,
                        FirstName = "Student2",
                        LastName = "2",
                        Email = studentEmail,
                        RegDate = DateTime.Now,
                        CourseId = CourseId

                    };

                    //--------LÄGG TILL STUDENT2--------
                    var addTeacherResult = await userManager.CreateAsync(user, studentPW);
                    if (!addTeacherResult.Succeeded) throw new Exception(string.Join("\n", addTeacherResult.Errors));
                }

                //--------GE ROLL STUDENT2--------
                student = await userManager.FindByNameAsync(studentEmail);

                if (!await userManager.IsInRoleAsync(student, "Student"))
                {
                    var addToRoleResult = await userManager.AddToRoleAsync(student, "Student");
                    if (!addToRoleResult.Succeeded) throw new Exception(string.Join("\n", addToRoleResult.Errors));
                }

                //--------SKAPA STUDENT2--------
                //var studentPW = "abc123";
                studentEmail = "student3@lms.se";
                studentUser = await userManager.FindByEmailAsync(studentEmail);
                if (studentUser == null)
                {
                    var user = new User
                    {
                        UserName = studentEmail,
                        FirstName = "Student3",
                        LastName = "3",
                        Email = studentEmail,
                        RegDate = DateTime.Now,
                        CourseId = CourseId

                    };

                    //--------LÄGG TILL STUDENT2--------
                    var addTeacherResult = await userManager.CreateAsync(user, studentPW);
                    if (!addTeacherResult.Succeeded) throw new Exception(string.Join("\n", addTeacherResult.Errors));
                }

                //--------GE ROLL STUDENT2--------
                student = await userManager.FindByNameAsync(studentEmail);

                if (!await userManager.IsInRoleAsync(student, "Student"))
                {
                    var addToRoleResult = await userManager.AddToRoleAsync(student, "Student");
                    if (!addToRoleResult.Succeeded) throw new Exception(string.Join("\n", addToRoleResult.Errors));
                }



            }

        }
    }
}
