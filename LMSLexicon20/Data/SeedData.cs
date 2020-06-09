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

            using (var context = new ApplicationDbContext(options))
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
                var teacherEmail = "teacher1@lms.se";
                var teacherUser = await userManager.FindByEmailAsync(teacherEmail);
                if (teacherUser == null)
                {
                    var user = new User
                    {
                        UserName = teacherEmail,
                        FirstName = "Teacher",
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

            }

        }
    }
}
