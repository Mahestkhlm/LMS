using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMSLexicon20.Data;
using LMSLexicon20.Models;
using LMSLexicon20.ViewModel;
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

        public UsersController(ApplicationDbContext dbContext, UserManager<User> userManager)
        {
            _context = dbContext;
            _userManager = userManager;
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

        // POST: User/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Teacher")]
        //ToDo: add attributes (phoneNumber, email...)
        //ToDo: rätt namn?
        //public async Task<IActionResult> CreateUser(UserViewModel viewModel)
        //{
        //    //ToDo:add mapping
        //    if (ModelState.IsValid)
        //    {
        //        var model = _mapper.Map<User>(viewModel);

        //        //if courseid exist -> student
                
        //        //var courseId = viewModel.CourseId;
        //        int? courseId = model.CourseId;
                
        //            var teacher = await _userManager.FindByNameAsync(model.UserName);
        //            var addRoleResult= _userManager.AddToRoleAsync(teacher, "Teacher");
        //            if(!addRoleResult.Succeded) throw new Exception(string.Join("\n", addToRoleResult.Errors));
                
        //            //var student = await _userManager.FindByNameAsync(model.UserName);
        //            //var addRoleResult= _userManager.AddToRoleAsync(student, "Student");
        //            //if (!addRoleResult.Succeded) throw new Exception(string.Join("\n", addToRoleResult.Errors))
                
        //        _context.Add(model);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(viewModel);

    //    }
   }
}
