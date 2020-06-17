using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMSLexicon20.Data;
using LMSLexicon20.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using LMSLexicon20.Models.ViewModels;

namespace LMSLexicon20.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public ActivitiesController(ApplicationDbContext context ,IMapper mapper )
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: Activities
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Index()
        {
            //var applicationDbContext = _context.Activities.Include(a => a.ActivityType).Include(a => a.Module);
            //return View(await applicationDbContext.ToListAsync());

            var model = await mapper.ProjectTo<ActivityListViewModel>(_context.Activities).ToListAsync();
            return View(model);

        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities
                .Include(a => a.ActivityType)
                .Include(a => a.Module)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // GET: Activities/Create
        [Authorize(Roles = "Teacher")]
        public IActionResult Create(int? id)
        {
            //ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "Name");
            //ViewData["ModuleId"] = new SelectList(_context.Set<Module>(),"Id", "Name", ModuleId);
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Create(CreateActivityViewModel activity, int id)
        {
            if (activity.EndDate < activity.StartDate)
            {
                ModelState.AddModelError("EndDate", "Sluttiden kan inte vara tidigare än starttiden");
            }


            if (ModelState.IsValid)
            {
                var model = mapper.Map<Activity>(activity);
                model.ModuleId = id;

                _context.Add(model);
                await _context.SaveChangesAsync();
                TempData["SuccessText"] = $"Aktivitet- {model.Name} - är skapad!";

                if (model.ModuleId == default) return RedirectToAction(nameof(Index));
                var CourseId = _context.Modules.Where(m => m.Id == model.ModuleId).Select(m => m.CourseId).FirstOrDefault();
                return RedirectToAction(nameof(Details), "Courses", new { id = CourseId });
            }
            return View(activity);
        }

        // GET: Activities/Edit/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var activity = await mapper.ProjectTo<ActivityEditViewModel>(_context.Activities).FirstOrDefaultAsync(e => e.Id == id);
            //var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "Name", activity.ActivityTypeId);
            ViewData["ModuleId"] = new SelectList(_context.Set<Module>(), "Id", "Name", activity.ModuleId);
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int id,  ActivityEditViewModel activity)
        {
            
            if (id != activity.Id)
            {
                return NotFound();
            }

            if (activity.EndDate < activity.StartDate)
            {
                ModelState.AddModelError("EndDate", "Sluttiden kan inte vara tidigare än starttiden");
            }
           /* var found = await _context.Courses.AnyAsync(p => (p.Name == activity.Name) && (p.Id != activity.Id));
            if (found)
            {
                ModelState.AddModelError("Name", "Det finns redan en kurs med denna namn");
            }*/

            var model = mapper.Map<Activity>(activity);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["SuccessText"] = $"Aktivitet: {activity.Name} - är uppdaterad!";
                return RedirectToAction(nameof(Index));
            }

            TempData["FailText"] = $"Något gick fel! Aktivitet: {activity.Name} - är inte uppdaterad!";

            return View(activity);
        }

        // GET: Activities/Delete/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await mapper.ProjectTo<DeleteActivityViewModel>(_context.Activities).FirstOrDefaultAsync(t => t.Id == id);
            //var activity = await _context.Activities
                //.Include(a => a.ActivityType)
                //.Include(a => a.Module)
                //.FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity); ;
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ModuleId = _context.Activities.Where(a => a.Id == id).Select(a => a.ModuleId).FirstOrDefault();
            var CourseId = _context.Modules.Where(m => m.Id == ModuleId).Select(m => m.CourseId).FirstOrDefault();

            var model = await mapper.ProjectTo<DeleteActivityViewModel>(_context.Activities).FirstOrDefaultAsync(t => t.Id == id);
            var activity = await _context.Activities.FindAsync(id);
            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            TempData["SuccessText"] = $"Aktivitet: {activity.Name} - är raderad!";
            return RedirectToAction(nameof(Details), "Courses", new { id = CourseId });
        }

        private bool ActivityExists(int id)
        {
            return _context.Activities.Any(e => e.Id == id);
        }


        private bool ModuleNotEmpty()
        {
            return (!_context.Modules.Any());
        }

    }
}
