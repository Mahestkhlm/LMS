using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LMSLexicon20.Controllers
{
    public class UsersController : Controller
    {
        private object _context;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Start()
        {
            return View();
        }



        
    }
}
