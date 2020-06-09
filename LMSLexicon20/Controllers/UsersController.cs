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
        public ActionResult CreateUser()
        {
            return View();
        }

        //POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        //ToDo: add attributes(phoneNumber, email...)
        //ToDo: rätt namn?
        public async Task<IActionResult> CreateUser(CreateUserViewModel viewModel, int? id=null)
        {
            //ToDo:add mapping
            if (ModelState.IsValid)
            {
                
                var model = _mapper.Map<User>(viewModel);
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null) throw new Exception("User already exists");

                if (id != null)
                {
                    //var courseId = model.CourseId;
                    model.Course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
                    //finns inte kurs 
                }

                //ToDo: generate pw
                var pw = GeneratePassword();
                //ToDo: show password in view
                var addUserResult = await _userManager.CreateAsync(model, pw);
                if (!addUserResult.Succeeded) throw new Exception(string.Join("\n", addUserResult.Errors));

                var addRoleResult = id == null ?
                await _userManager.AddToRoleAsync(model, "Teacher") :        //true=teacher
                await _userManager.AddToRoleAsync(model, "Student");         //false=student

                if (!addRoleResult.Succeeded) throw new Exception(string.Join("\n", addRoleResult.Errors));

                //_context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            return View(viewModel);

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
            return Json(_context.Users.Any(u => u.Email == Email));
        }

    }
}
