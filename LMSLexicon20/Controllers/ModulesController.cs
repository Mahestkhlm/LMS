using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMSLexicon20.Data;
using LMSLexicon20.Models;
using AutoMapper;
using LMSLexicon20.Models.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;

namespace LMSLexicon20.Controllers
{
    public class ModulesController : Controller
    {
        private ApplicationDbContext context;
        private readonly IMapper mapper;

        public ModulesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET: Modules
        public async Task<IActionResult> Index()
        {
            return View(await context.Modules.Include(m => m.Course).ToListAsync());
        }

        // GET: Modules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await context.Modules
                .Include(m => m.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Modules/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(context.Courses, "Id", "Id");
            return View();
        }
        //Get
        public IActionResult CreateModule(int? id)
        {
            //ToDo: koppla kurs

            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,EndDate,Description,CourseId")] Module model)
        {
            if (ModelState.IsValid)
            {
                context.Add(model);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(context.Courses, "Id", "Id", model.CourseId);
            return View(model);
        }
        //Post
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public async Task<IActionResult> CreateModule(CreateModuleViewModel viewModel, int id)
        {
            if (ModelState.IsValid)
            {
                var model = mapper.Map<Module>(viewModel);
                model.CourseId = id;

                //ToDo: kolla eller inte
                //var moduleExists = await context.Modules.AnyAsync(m => m.id == ?);
                //if (!module)

                //ToDo: kan man ha FÖR många async?
                await context.Modules.AddAsync(model);
                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Modules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await context.Modules.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(context.Courses, "Id", "Id", model.CourseId);
            return View(model);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,EndDate,Description,CourseId")] Module model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(model);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleExists(model.Id))
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
            ViewData["CourseId"] = new SelectList(context.Courses, "Id", "Id", model.CourseId);
            return View(model);
        }

        // GET: Modules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await context.Modules
                .Include(m => m.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await context.Modules.FindAsync(id);
            context.Modules.Remove(model);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModuleExists(int id)
        {
            return context.Modules.Any(e => e.Id == id);
        }
    }
}
