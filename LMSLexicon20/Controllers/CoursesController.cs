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

namespace LMSLexicon20.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext db;

        public CoursesController(ApplicationDbContext context)
        {
            db = context;

            var CourseId = db.Courses.Where(c => c.Name == ".Net").Select(c => c.Id).FirstOrDefault();
            if (db.Courses.Find(CourseId)?.Id is null)
            {
                db.Courses.Add(new Course {  Name = ".Net", StartDate = new DateTime(2020, 6, 27, 20, 0, 0), Description = "I den här självstudien visas hur du skapar en .NET Core-app och ansluter den till SQL Database. När du är klar har du en .NET Core MVC-app som körs i App Service" });
                db.Courses.Add(new Course {  Name = "Azure", StartDate = new DateTime(2020, 5, 27, 20, 0, 0), Description = "Automatically deploy and update a static web application and its API from a GitHub repository.\nIn this module, you will:\nChoose an existing web app project with either Angular,\nReact,\nSvelte or Vue\nCreate an API for the app with Azure Functions\nRun the application locally\nPublish the app and its API to Azure Static Web Apps" });
                db.SaveChanges();
                CourseId = db.Courses.Where(c => c.Name == ".Net").Select(c => c.Id).FirstOrDefault();
            }
            var ModuleId = db.Modules.Where(m => m.Name == "Azure deploy").Select(m => m.Id).FirstOrDefault();
            if (db.Modules.Find(ModuleId)?.Id is null)
            {
                db.Modules.Add(new Module { CourseId = CourseId, Name = "Azure deploy", StartDate = new DateTime(2020, 6, 27, 20, 0, 0), Description = "Deploy a website to Azure with Azure App Service" });
                db.Modules.Add(new Module { CourseId = CourseId, Name = "Azure Well", StartDate = new DateTime(2020, 6, 27, 20, 0, 0), Description = "Build great solutions with the Microsoft Azure Well - Architected Framework" });
                db.SaveChanges();
                ModuleId = db.Modules.Where(m => m.Name == "Azure deploy").Select(m => m.Id).FirstOrDefault();
            }

            var ActivityTypeId = db.ActivityTypes.Where(a => a.Name == "e-learningpass").Select(m => m.Id).FirstOrDefault();
            if (db.ActivityTypes.Find(ActivityTypeId)?.Id is null)
            {
                db.ActivityTypes.Add(new ActivityType { Name = "e-learningpass" });
                db.ActivityTypes.Add(new ActivityType { Name = "föreläsningar" });
                db.ActivityTypes.Add(new ActivityType { Name = "övningstillfällen" });
                db.ActivityTypes.Add(new ActivityType { Name = "annat" });
                db.SaveChanges();
                ActivityTypeId = db.ActivityTypes.Where(a => a.Name == "e-learningpass").Select(m => m.Id).FirstOrDefault();
            }

            var ActivityId = db.Activities.Where(a => a.Name == "Environment").Select(m => m.Id).FirstOrDefault();
            if (db.Activities.Find(ActivityId)?.Id is null)
            {
                db.Activities.Add(new Activity { ModuleId = ModuleId, ActivityTypeId= ActivityTypeId, Name = "Environment", StartDate = new DateTime(2020, 6, 27, 20, 0, 0), Description = "Prepare your development environment for Azure development" });
                db.Activities.Add(new Activity { ModuleId = ModuleId, ActivityTypeId = ActivityTypeId, Name = "App service", StartDate = new DateTime(2020, 5, 27, 20, 0, 0), Description = "Host a web application with Azure App service" });
                db.Activities.Add(new Activity { ModuleId = ModuleId, ActivityTypeId = ActivityTypeId, Name = "Web app platform", StartDate = new DateTime(2020, 5, 27, 20, 0, 0), Description = "Learn how to create a website through the hosted web app platform in Azure App Service" });
                db.SaveChanges();
            }
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            return View(await db.Courses.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //

            //db.Courses.Find(1)
            //await 
            var courseDetailVM = db.Courses
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
                //var courseTmp = db.Courses.FirstOrDefaultAsync(c => c.Id == id);
                return NotFound();
            }

            return View(nameof(Details),await courseDetailVM);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,EndDate,Description")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Add(course);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await db.Courses.FindAsync(id);
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
                    db.Update(course);
                    await db.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await db.Courses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await db.Courses.FindAsync(id);
            db.Courses.Remove(course);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return db.Courses.Any(e => e.Id == id);
        }
    }
}
