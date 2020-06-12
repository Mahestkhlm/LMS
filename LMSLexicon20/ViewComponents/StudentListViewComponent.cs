using LMSLexicon20.Data;
using LMSLexicon20.Models;
using LMSLexicon20.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewComponentSample.ViewComponents
{
    public class StudentListViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext db;
        private UserManager<User> _userManager;

        public StudentListViewComponent(ApplicationDbContext context, UserManager<User> userManager)
        {
            db = context;
            _userManager = userManager;
        }


        public async Task<IViewComponentResult> InvokeAsync(int IdCourse)
        {
            var items = await GetItemsAsync(IdCourse);
            return View(items);
        }

        //
        private async Task<List<StudentVM>> GetItemsAsync(int IdCourse)
        {
            var students = await _userManager.GetUsersInRoleAsync("Student");

            return students  //db.Users
                .Where(u => u.CourseId == IdCourse)
                .Select(c => new StudentVM
                {
                    Id = c.Id,
                    Email = c.Email,
                    FullName = c.FirstName + " " + c.LastName
                })
                .ToList();
                //.ToListAsync();
        }
    }
}