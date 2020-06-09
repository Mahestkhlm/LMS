using LMSLexicon20.Data;
using LMSLexicon20.Models.ViewModels;
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

        public StudentListViewComponent(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int IdCourse)
        {
            var items = await GetItemsAsync(IdCourse);
            return View(items);
        }

        //
        private Task<List<StudentVM>> GetItemsAsync(int IdCourse)
        {
            return db.Users
                .Where(u => u.CourseId == IdCourse)
                .Select(c => new StudentVM
                {
                    Id = c.Id,
                    Email = c.Email,
                    FullName = c.FirstName + " " + c.LastName
                })
                .ToListAsync();
        }
    }
}