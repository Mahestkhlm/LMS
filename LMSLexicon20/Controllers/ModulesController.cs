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
            var modules = await context.Modules.Include(m => m.Course).ToListAsync();
            var viewModel = await mapper.ProjectTo<IndexModuleViewModel>
                (context.Modules.Include(m => m.Course)).ToListAsync();
            return View(viewModel);
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
            var viewModel = mapper.Map<DetailModuleViewModel>(model);
            viewModel.Id = id;
            return View(viewModel);
        }
        //Get
        public IActionResult CreateModule(int id)
        {
            TempData["courseId"] = id;
            return View();
        }
        //Post
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,StartDate,EndDate,Description,CourseId")] Module model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        context.Add(model);
        //        await context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CourseId"] = new SelectList(context.Courses, "Id", "Id", model.CourseId);
        //    return View(model);
        //}
        ////Post
        //[HttpPost]
        [Authorize(Roles = "Teacher")]
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
                TempData["SuccessText"] = $"Modulen: {model.Name} - är skapad!";
                return RedirectToAction(nameof(Details), "Courses", new { id = model.CourseId });
            }
            return View(viewModel);
        }

        // GET: Modules/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            //ToDo: nullcheck?
            //För att kunna gå tillbaka till kurs
            var model = await context.Modules.FindAsync(id);
            var viewModel = mapper.Map<EditModuleViewModel>(model);
            TempData["CourseId"] = model.CourseId;
            return View(viewModel);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditModuleViewModel viewModel, int id)
        {

            var found = await context.Modules
                .Where(m=>m.CourseId==viewModel.CourseId)
                .AnyAsync(p => (p.Name == viewModel.Name) 
                && (p.Id != id));
            if (found)
            {
                ModelState.AddModelError("Name", "Namnet används redan i denna kurs");
            }

            if (ModelState.IsValid)
            {
                var model = await context.Modules.FindAsync(id);
               
                model.Name = viewModel.Name;
                model.StartDate = viewModel.StartDate;
                model.EndDate = viewModel.EndDate;
                model.Description = viewModel.Description;
                //ToDo: behövs den?
                context.Entry(model).Property(p => p.CourseId).IsModified = false;

                try
                {
                    context.Update(model);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleExists(model.Id))
                        return NotFound();
                    else
                        throw;
                }
                TempData["SuccessText"] = $"Modulen {model.Name} har uppdaterats";
                return RedirectToAction(nameof(Details), "Courses", new { id = model.CourseId });
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
            var viewModel = mapper.Map<DeleteModuleViewModel>(model);
            return View(viewModel);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await context.Modules.FindAsync(id);
            var name = model.Name;
            context.Modules.Remove(model);
            await context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            TempData["SuccessText"] = $"Modulen {name} har tagits bort";
            return RedirectToAction(nameof(Details), "Courses", new { id = id });
        }

        private bool ModuleExists(int id)
        {
            return context.Modules.Any(e => e.Id == id);
        }
    }
}
