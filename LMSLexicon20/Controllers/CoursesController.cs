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
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace LMSLexicon20.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public CoursesController(ApplicationDbContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        // GET: Courses
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Index(string filterSearch, string sortOrder)
        {
            var viewModel = await mapper.ProjectTo<CourseIndexViewModel>(_context.Courses).ToListAsync();

            var filter = string.IsNullOrWhiteSpace(filterSearch) ?
                              viewModel : viewModel.Where(m => m.Name.ToLower().Contains(filterSearch.ToLower()));

            ViewData["OptionOne"] = sortOrder == "name_desc" ? "Name" : "name_desc";
            ViewData["OptionTwo"] = sortOrder == "EndDate" ? "enddate_desc" : "EndDate";
            ViewData["OptionThree"] = sortOrder == "StartDate" ? "startdate_desc" : "StartDate";

            filter = sortOrder switch
            {
                "EndDate" => filter.OrderBy(s => s.EndDate),
                "enddate_desc" => filter.OrderByDescending(s => s.EndDate),
                "StartDate" => filter.OrderBy(s => s.StartDate),
                "startdate_desc" => filter.OrderByDescending(s => s.StartDate),
                "name_desc" => filter.OrderByDescending(s => s.Name),
                _ => filter.OrderBy(s => s.Name),
            };

            return View(filter);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teachers = await userManager.GetUsersInRoleAsync("Teacher");
            var courseTeacher = teachers.FirstOrDefault(e => e.CourseId == id);

            //_context.Courses.Find(1)
            //await 
            var courseDetailVM = _context.Courses
                    //.Include(c => c.Modules)
                    //.ThenInclude(m => m.Activities)
                    .OrderBy(m => m.StartDate)
                    .Select(c => new CourseDetailVM
                    {
                        Id = c.Id,
                        Name = c.Name,
                        StartDate = c.StartDate,
                        EndDate = c.EndDate,
                        Description = c.Description,
                        Teacher = courseTeacher
                        ,
                        ModuleDetailVM = (ICollection<ModuleDetailVM>)c.Modules
                                    .OrderBy(c => c.StartDate)
                                    .Select(m => new ModuleDetailVM
                                    {
                                       Id = m.Id,
                                       Name = m.Name,
                                       StartDate = m.StartDate,
                                       StartDateToEarly = (m.StartDate < c.StartDate),
                                       StartDateToLate = (m.StartDate > c.EndDate),
                                       StartDateOverlap = c.Modules.Where(m2 => m.StartDate > m2.StartDate && m.StartDate < m2.EndDate).Any(),
                                       EndDate = m.EndDate,
                                       EndDateToEarly = (m.EndDate < c.StartDate),
                                       EndDateToLate = (m.EndDate > c.EndDate),
                                       EndDateOverlap = c.Modules.Where(m2 => m.EndDate > m2.StartDate && m.EndDate < m2.EndDate).Any(),
                                       Description = m.Description,
                                       Expanded = (DateTime.Now > m.StartDate &&  DateTime.Now<m.EndDate)
                                       ,
                                       ActivityDetailVM = (ICollection<ActivityDetailVM>)m.Activities
                                            .OrderBy(a => a.StartDate)
                                            .Select(a => new ActivityDetailVM
                                            {
                                                Id = a.Id,
                                                Name = a.Name,
                                                StartDate = a.StartDate,
                                                StartDateToEarly = (a.StartDate < m.StartDate),
                                                StartDateToLate = (a.StartDate > m.EndDate),
                                                StartDateOverlap = m.Activities.Where(a2 => a.StartDate > a2.StartDate && a.StartDate < a2.EndDate).Any(),
                                                EndDate = a.EndDate,
                                                EndDateToEarly = (a.EndDate < m.StartDate),
                                                EndDateToLate = (a.EndDate > m.EndDate),
                                                EndDateOverlap = m.Activities.Where(a2 => a.EndDate > a2.StartDate && a.EndDate < a2.EndDate).Any(),
                                                Description = a.Description,
                                                Expanded = (DateTime.Now > a.StartDate && DateTime.Now < a.EndDate)
                                                ,
                                                ActivityTypeWM =
                                                new ActivityTypeWM
                                                {
                                                    Id = a.ActivityType.Id,
                                                    Name = a.ActivityType.Name,
                                                    RequireDocument = a.ActivityType.RequireDocument
                                                }
                                            })
                                   })
                    })
                    .FirstOrDefaultAsync(c => c.Id == id);


            if (courseDetailVM == null)
            {
                //var courseTmp = _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
                return NotFound();
            }

            return View(nameof(Details), await courseDetailVM);
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
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Create(CreateCourseViewModel viewModel)
        {
            if (viewModel.StartDate >= viewModel.EndDate)
            {
                ModelState.AddModelError("EndDate", "Kursen kan inte avsluta innan den börjar");
            }

            if (ModelState.IsValid)
            {
                var model = mapper.Map<Course>(viewModel);

                _context.Add(model);
                await _context.SaveChangesAsync();
                TempData["SuccessText"] = $"Kursen: {model.Name} - är skapad!";
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }



        // GET: Courses/Edit/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await mapper.ProjectTo<EditCourseViewModel>(_context.Courses).FirstOrDefaultAsync(e => e.Id == id);
            var teachers = await userManager.GetUsersInRoleAsync("Teacher");
            course.Teacher = teachers.FirstOrDefault(e => e.CourseId == id);

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
        public async Task<IActionResult> Edit(int id, EditCourseViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (viewModel.StartDate >= viewModel.EndDate)
            {
                ModelState.AddModelError("EndDate", "Kursen kan inte avsluta innan den börjar");
            }

            var found = await _context.Courses.AnyAsync(p => (p.Name == viewModel.Name) && (p.Id != viewModel.Id));
            if (found)
            {
                ModelState.AddModelError("Name", "Det finns redan en kurs med denna namn");
            }

            var model = mapper.Map<Course>(viewModel);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["SuccessText"] = $"Kursen: {model.Name} - är uppdaterad!";
                return RedirectToAction(nameof(Index));
            }
            TempData["FailText"] = $"Något gick fel! Kursen: {model.Name} - är inte uppdaterad!";

            return View(viewModel);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await mapper.ProjectTo<DeleteCourseViewModel>(_context.Courses).FirstOrDefaultAsync(e => e.Id == id);
            var teachers = await userManager.GetUsersInRoleAsync("Teacher");
            course.Teacher = teachers.FirstOrDefault(e => e.CourseId == id);
            var students = await userManager.GetUsersInRoleAsync("Student");
            course.Students = students.Where(e => e.CourseId == id).ToList();
            
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

            var teachers = await userManager.GetUsersInRoleAsync("Teacher");
            var teacher = teachers.FirstOrDefault(e => e.CourseId == id);
            if (teacher != null) teacher.CourseId = null;

            var students = await userManager.GetUsersInRoleAsync("Student");
            var courseStudents = students.Where(e => e.CourseId == id).ToList();
            foreach (var item in courseStudents)
            {
                _context.Users.Remove(item);
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            TempData["SuccessText"] = $"Kursen: {course.Name} - är raderad!";
            return RedirectToAction(nameof(Index));
        }
       


        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
        [HttpPost]
        public JsonResult DoesCourseExist(int CourseId)
        {
            var courseExists = _context.Courses.Any(c => c.Id == CourseId);
            return Json(courseExists);
        }

        public IActionResult IsNameUnique(string name)
        {
            var result = _context.Courses.Any(s => s.Name.ToLower() == name.ToLower());
            return Json(!result);
        }

    }
}
