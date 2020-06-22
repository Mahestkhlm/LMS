using LMSLexicon20.Data;
using LMSLexicon20.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LMSLexicon20.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public DocumentsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Documents
        public async Task<IActionResult> Index()
        {
            var lMSLexicon20Context = _context.Documents.Include(d => d.Activity).Include(d => d.Course).Include(d => d.Module).Include(d => d.User);
            return View(await lMSLexicon20Context.ToListAsync());
        }

        // GET: Documents/Details/5

        public async Task<IActionResult> CurrentUser()
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            var teacher = await _userManager.GetUsersInRoleAsync("Teacher");
            List<Document> documents;
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (teacher.Where(s => s.Id == UserId).Any())
            {
                var CourseId = _context.Users
                    .Where(u => u.Id == UserId)
                    .Select(u => u.CourseId)
                    .FirstOrDefault();

                //var mergedList = list1.Union(list2).ToList();

                documents = _context.Documents
                    .Where(d => d.CourseId == CourseId)
                    .ToList()
                .Union(
                    _context.Documents
                    .Where(d => d.CourseId == CourseId)
                    ).ToList()
                .Union(
                    _context.Documents
                    .Where(d => d.CourseId == CourseId)
                    ).ToList();

                //.Where(c => c.Id == )
                //.Include(d => d.Module)
                //.Include(d => d.Activity)
            }
            else
            {
                //var students = await _userManager.GetUsersInRoleAsync("Student");
                documents = await _context.Documents
                    .Where(d => d.UserId == UserId)
                    .ToListAsync();

                //.Include(d => d.Course)
                //.ThenInclude(c => c.Modules)
                //.ThenInclude(m => m.Activities)
            }

            if (documents == null)
            {
                return NotFound();
            }

            return View(nameof(Index), documents);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .Include(d => d.Activity)
                .Include(d => d.Course)
                .Include(d => d.Module)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Documents/Create
        public IActionResult Create()
        {
            ViewData["ActivityId"] = new SelectList(_context.Set<Activity>(), "Id", "Id");
            ViewData["CourseId"] = new SelectList(_context.Set<Course>(), "Id", "Id");
            ViewData["ModuleId"] = new SelectList(_context.Set<Module>(), "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Id");
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Date,Path,UserId,CourseId,ModuleId,ActivityId")] Document document)
        {
            if (ModelState.IsValid)
            {
                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityId"] = new SelectList(_context.Set<Activity>(), "Id", "Id", document.ActivityId);
            ViewData["CourseId"] = new SelectList(_context.Set<Course>(), "Id", "Id", document.CourseId);
            ViewData["ModuleId"] = new SelectList(_context.Set<Module>(), "Id", "Id", document.ModuleId);
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Id", document.UserId);
            return View(document);
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            ViewData["ActivityId"] = new SelectList(_context.Set<Activity>(), "Id", "Id", document.ActivityId);
            ViewData["CourseId"] = new SelectList(_context.Set<Course>(), "Id", "Id", document.CourseId);
            ViewData["ModuleId"] = new SelectList(_context.Set<Module>(), "Id", "Id", document.ModuleId);
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Id", document.UserId);
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Date,Path,UserId,CourseId,ModuleId,ActivityId")] Document document)
        {
            if (id != document.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.Id))
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
            ViewData["ActivityId"] = new SelectList(_context.Set<Activity>(), "Id", "Id", document.ActivityId);
            ViewData["CourseId"] = new SelectList(_context.Set<Course>(), "Id", "Id", document.CourseId);
            ViewData["ModuleId"] = new SelectList(_context.Set<Module>(), "Id", "Id", document.ModuleId);
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Id", document.UserId);
            return View(document);
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .Include(d => d.Activity)
                .Include(d => d.Course)
                .Include(d => d.Module)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var document = await _context.Documents.FindAsync(id);

            //var course = await _context.Courses.FindAsnc(document.CourseId);
            //_context.Courses.Remove(course);
            //await _context.SaveChangesAsync();

            //var module = await _context.Modules.FindAsync(document.ModuleId);
            //_context.Modules.Remove(module);
            //await _context.SaveChangesAsync();

            //var activity = await _context.Activities.FindAsync(document.ActivityId);
            //_context.Activities.Remove(activity);
            //await _context.SaveChangesAsync();

            //var user = await _context.Users.FindAsync(document.UserId);
            //_context.Users.Remove(user);
            //await _context.SaveChangesAsync();

            //document.CourseId = null;
            //_context.Update(document);
            //await _context.SaveChangesAsync();

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();

            var path = Path.Combine( "wwwroot",document.Path);
            System.IO.File.Delete(Path.GetFullPath(path));
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.Id == id);
        }
    }
}