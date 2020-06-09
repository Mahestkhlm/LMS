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

            var CourseId = _context.Courses.Where(c => c.Name == ".Net").Select(c => c.Id).FirstOrDefault();
            if (_context.Courses.Find(CourseId)?.Id is null)
            {
                _context.Courses.Add(new Course {  Name = ".Net", StartDate = new DateTime(2020, 6, 27, 20, 0, 0), Description = "I den här självstudien visas hur du skapar en .NET Core-app och ansluter den till SQL Database. När du är klar har du en .NET Core MVC-app som körs i App Service" });
                _context.Courses.Add(new Course {  Name = "Azure", StartDate = new DateTime(2020, 5, 27, 20, 0, 0), Description = "Automatically deploy and update a static web application and its API from a GitHub repository.\nIn this module, you will:\nChoose an existing web app project with either Angular,\nReact,\nSvelte or Vue\nCreate an API for the app with Azure Functions\nRun the application locally\nPublish the app and its API to Azure Static Web Apps" });
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
                _context.Activities.Add(new Activity { ModuleId = ModuleId, ActivityTypeId= ActivityTypeId, Name = "Environment", StartDate = new DateTime(2020, 6, 27, 20, 0, 0), Description = "Prepare your development environment for Azure development" });
                _context.Activities.Add(new Activity { ModuleId = ModuleId, ActivityTypeId = ActivityTypeId, Name = "App service", StartDate = new DateTime(2020, 5, 27, 20, 0, 0), Description = "Host a web application with Azure App service" });
                _context.Activities.Add(new Activity { ModuleId = ModuleId, ActivityTypeId = ActivityTypeId, Name = "Web app platform", StartDate = new DateTime(2020, 5, 27, 20, 0, 0), Description = "Learn how to create a website through the hosted web app platform in Azure App Service" });
                _context.SaveChanges();
            }
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
