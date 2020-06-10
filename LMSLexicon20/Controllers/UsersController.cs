using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

using LMSLexicon20.Data;
using LMSLexicon20.Models;
using LMSLexicon20.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace LMSLexicon20.Controllers
{
    public class UsersController : Controller
    {

        private ApplicationDbContext _context;
        private UserManager<User> _userManager;
        private IMapper _mapper;

        public UsersController(ApplicationDbContext dbContext, UserManager<User> userManager, IMapper mapper)
        {
            _context = dbContext;
            _userManager = userManager;
            _mapper = mapper;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Start()
        {
            return View();
        }


        // GET: User/Create
        [Authorize(Roles = "Teacher")]
        public ActionResult CreateUser(int? courseId = null)
        {
            //courseId sätts inte som /Users/CreateUser/1 men /Users/CreateUser?courseId=1
            //Den funkar för det andra. Mest jag som ville prova!
            if (courseId != null)
            {
                var courseExists = _context.Courses.Any(c => c.Id == courseId);
                if (!courseExists)
                    throw new Exception("Du försökte gå till en kurs som inte finns!");
            }
            return View();
        }

        //POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        //ToDo: add attributes(phoneNumber, email...)
        //ToDo: rätt namn?
        public async Task<IActionResult> CreateUser(CreateUserViewModel viewModel, int? id = null)
        {
            //ToDo: fråga Dimitris om att döpa om asp-route-values till nåt annat än id (ex. courseId)
            if (ModelState.IsValid)
            {
                //Hämta användare
                var model = _mapper.Map<User>(viewModel);
                var user = await _userManager.FindByNameAsync(model.UserName);
                //Bör aldrig vara null - checkar ändå
                if (user != null) throw new Exception("Användaren finns redan");

                //Lägg till kurs om finns (checkat att den finns)
                if (id != null) model.CourseId = id;
                //if (courseId != null) model.Course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);

                //ToDo: show password in view
                //TempData och sen in i vy

                //Lägg till användare m. lösen
                var pw = GeneratePassword();
                
                var addUserResult = await _userManager.CreateAsync(model, pw);
                if (!addUserResult.Succeeded) throw new Exception(string.Join("\n", addUserResult.Errors));

                //Lägg till roll
                var addRoleResult = id == null ?
                await _userManager.AddToRoleAsync(model, "Teacher") :        //true=teacher
                await _userManager.AddToRoleAsync(model, "Student");         //false=student
                if (!addRoleResult.Succeeded) throw new Exception(string.Join("\n", addRoleResult.Errors));
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CreateUserConfirmed), new { userName=model.UserName, pw = pw });
            }
            return View(viewModel);

        }
        public IActionResult CreateUserConfirmed(string userName, string pw)
        {
            TempData["pw"] = pw;
            TempData["userName"] = userName;
            return View();
        }
        static string GeneratePassword()
        {
            //ToDo: check final string
            var length = 15;
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            //var validUpperCases = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
            //var validLowerCases = "abcdefghijklmnopqrstuvwxyz";
            //var validNumbers = "0123456789";
            //var validSpecial = "!@#$%^&*?_-";

            Random random = new Random();

            char[] chars = new char[length];

            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> List(string filterSearch)
        {
            var viewModel = await _mapper.ProjectTo<UserListViewModel>(_userManager.Users).ToListAsync();
            for (int i = 0; i < viewModel.Count; i++)
            {
                var role = _userManager.GetRolesAsync(_userManager.FindByIdAsync(viewModel[i].Id).Result).Result[0];
                viewModel[i].UserRole = role == "Teacher" ? "Lärare" : "Elev";
            }

            var filter = string.IsNullOrWhiteSpace(filterSearch) ?
                            viewModel : viewModel.Where(m => m.FullName.ToLower().Contains(filterSearch.ToLower()) ||
                                                                m.Email.ToLower().Contains(filterSearch.ToLower()));
            return View(filter);

        }
        [HttpPost]
        public JsonResult EmailInUse(string Email)
        {
            return Json(_context.Users.Any(u => u.Email == Email) == false);
        }

    }
}
