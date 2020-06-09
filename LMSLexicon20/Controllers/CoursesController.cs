using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMSLexicon20.Data;
using LMSLexicon20.Models;
using LMSLexicon20.ViewModels;
using Microsoft.AspNetCore.Authorization;
using LMSLexicon20.Models.ViewModels;

namespace LMSLexicon20.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var model = _context.Courses
                   .Select(c => new CourseIndexViewModel
                   {
                       Id = c.Id,
                       Name = c.Name,
                       Description = c.Description,
                       StartDate = c.StartDate,
                       EndDate = c.EndDate
                   });

            return View(await model.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //

            //_context.Courses.Find(1)
            //await 
            var courseDetailVM = _context.Courses
                //.Include(c => c.Modules)
                //.ThenInclude(m => m.Activities)
                    .Select(c => new CourseDetailVM
                    {
                        Id = c.Id,
                        Name = c.Name,
                        StartDate = c.StartDate,
                        EndDate = c.EndDate,
                        Description = c.Description
                        ,
                        ModuleDetailVM = (ICollection<ModuleDetailVM>)c.Modules
                                   .Select(m => new ModuleDetailVM
                                   {
                                       Id = m.Id,
                                       Name = m.Name,
                                       StartDate = m.StartDate,
                                       EndDate = m.EndDate,
                                       Description = m.Description
                                       ,
                                       ActivityDetailVM = (ICollection<ActivityDetailVM>)m.Activities
                                            .Select(a => new ActivityDetailVM
                                            {
                                                Id = a.Id,
                                                Name = a.Name,
                                                StartDate = a.StartDate,
                                                EndDate = a.EndDate,
                                                Description = a.Description
                                            })
                                   })
                    })
                    .FirstOrDefaultAsync(c => c.Id == id);


            if (courseDetailVM == null)
            {
                //var courseTmp = _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
                return NotFound();
            }

            return View(nameof(Details),await courseDetailVM);
        }

        // GET: Courses/Create
        [Authorize(Roles = "Teacher")]

        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,EndDate,Description")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                TempData["SuccessText"] = $"The Course: {course.Name} is Created!";
                return RedirectToAction(nameof(Index));
            }
            TempData["FailText"] = "Try Again! Something Went wrong!!";
            return View(course);
        }



        // GET: Courses/Edit/5
       [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,EndDate,Description")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["SuccessText"] = $"The Course : {course.Name}is Updated!";
                return RedirectToAction(nameof(Index));
            }
            TempData["FailText"] = $"Something Went Wrong! The Course: {course.Name} is not updated!";

            return View(course);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Teacher")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [Authorize(Roles = "Teacher")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            TempData["SuccessText"] = $"The Course: {course.Name} is deleted!";
            return RedirectToAction(nameof(Index));
        }
        // Course / Filter
        public async Task<IActionResult> Filter(string CourseName)
        {
            var model = await _context.Courses.ToListAsync();

            model = string.IsNullOrWhiteSpace(CourseName) ?
                model :
                model.Where(p => p.Name.ToLower().Contains(CourseName.ToLower())).ToList();

            return View(nameof(Index), model);
        }



      
        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
        [HttpPost]
        public JsonResult DoesCourseExist(int CourseId)
        {
            var courseExists = _context.Courses.Any(c => c.Id == CourseId) ;
            return Json(courseExists);
        }

    }
}
