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
using LMSLexicon20.Extensions;
using LMSLexicon20.Filters;
using LMSLexicon20.ViewModels;

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
        [Authorize(Roles = "Teacher")]
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
        [Authorize(Roles = "Teacher")]
        public IActionResult CreateModule(int id)
        {
            TempData["courseId"] = id;
            if (Request.IsAjax())
                return PartialView("CreateModulePartialView");
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
        [ValidateAjax]
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

                if (Request.IsAjax())
                {
                    var ajaxModel = new ModuleDetailVM
                    {
                        Id = model.Id,
                        Name = model.Name,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        Description = model.Description,
                        CourseId = model.CourseId,
                        Expanded = false
                    };
                    
                    return PartialView("CreateModuleSuccessPartialView", ajaxModel);
                }

                TempData["SuccessText"] = $"Modulen: {model.Name} - är skapad!";
                return RedirectToAction(nameof(Details), "Courses", new { id = model.CourseId });
            }
            if (Request.IsAjax())
            {
                return PartialView("CreateModulePartialView", viewModel);
            }

            return View(viewModel);
        }

        // GET: Modules/Edit/5
        [Authorize(Roles = "Teacher")]
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
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(EditModuleViewModel viewModel, int id)
        {

            var found = await context.Modules
                .Where(m => m.CourseId == viewModel.CourseId)
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
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await context.Modules
                .Include(m => m.Course)
                .Include(m => m.Activities)
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
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var module = await context.Modules.FindAsync(id);
            //var activities = module.Activities;
            var activities = await context.Activities.Where(a => a.ModuleId == module.Id).ToListAsync();
            var courseId = module.CourseId;
            var name = module.Name;

            if (activities != null)
                foreach (var activity in activities) context.Activities.Remove(activity);
            context.Modules.Remove(module);
            await context.SaveChangesAsync();

            TempData["SuccessText"] = $"Modulen {name} har tagits bort";
            return RedirectToAction(nameof(Details), "Courses", new { id = courseId });
        }

        private bool ModuleExists(int id)
        {
            return context.Modules.Any(e => e.Id == id);
        }
    }
}
