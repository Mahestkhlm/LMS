using LMSLexicon20.Data;
using LMSLexicon20.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Services
{
    public class TeacherChoiceDropdown : ITeacherChoiceDropdown
    {
        private readonly UserManager<User> userManager;

        public TeacherChoiceDropdown(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IEnumerable<SelectListItem>> GetTeacherList()
        {
            var teachers = await userManager.GetUsersInRoleAsync("Teacher");
            var idleTeachers = teachers.Where(e => e.CourseId == null);

            return idleTeachers.Select(e => new SelectListItem { Text = $"{e.FirstName} {e.LastName}", Value = e.Id });
        }
    }
}
