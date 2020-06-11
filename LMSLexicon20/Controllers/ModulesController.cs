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
        public async Task<IActionResult> Details(int id)
        {
            //ToDo: ta in int eller int?
            var model = await context.Modules
                .Include(m => m.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        //Get
        public IActionResult CreateModule(int id)
        {
            TempData["courseId"] = id;
            return View();
        }
        //Post
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> CreateModule(CreateModuleViewModel viewModel, int id)
        {
            var nameExists = context.Modules
                .Where(m => m.CourseId == id)
                .Any(m => m.Name == viewModel.Name);
            if (nameExists)
                ModelState.AddModelError("Name", "Namnet används redan i denna kurs");

            if (ModelState.IsValid)
            {
                var model = mapper.Map<Module>(viewModel);
                model.CourseId = id;

                //ToDo: kan man ha FÖR många async?
                await context.Modules.AddAsync(model);
                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), "Courses", new { id = model.CourseId });
            }
            return View(viewModel);
        }

        // GET: Modules/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            //ToDo: nullcheck?
            //För att kunna gå tillbaka till kurs
            var module = await context.Modules.FindAsync(id);
            TempData["CourseId"] = module.CourseId;

            return View();
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditModuleViewModel viewModel, int id)
        {
            var nameExists = context.Modules
               .Where(m => m.CourseId == id)
               .Any(m => m.Name == viewModel.Name);
            if (nameExists)
                ModelState.AddModelError("Name", "Namnet används redan i denna kurs");

            if (ModelState.IsValid)
            {
                var model = await context.Modules.FindAsync(id);
                //model.CourseId = id;
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
                //ToDo: add tempdata success
                return RedirectToAction(nameof(Details),"Courses", new {id= model.CourseId });
            }
            return View(viewModel);
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
            //return RedirectToAction(nameof(Index));
            //ToDo: add tempdata success / confirmed-page
            return RedirectToAction(nameof(Details), "Courses", new { id = id });
        }

        private bool ModuleExists(int id)
        {
            return context.Modules.Any(e => e.Id == id);
        }
    }
}
