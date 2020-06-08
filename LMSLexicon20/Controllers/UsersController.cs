using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LMSLexicon20.Models;
using LMSLexicon20.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMSLexicon20.Controllers
{
    public class UsersController : Controller
    {
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public UsersController(IMapper mapper, UserManager<User> userManager)
        {
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Start()
        {
            return View();
        }

        public async Task<IActionResult> List(string filterSearch)
        {
            var viewModel = await mapper.ProjectTo<UserListViewModel>(userManager.Users).ToListAsync();
            
            var filter = string.IsNullOrWhiteSpace(filterSearch) ?
                            viewModel : viewModel.Where(m => m.FullName.ToLower().Contains(filterSearch.ToLower()) || 
                                                                m.Email.ToLower().Contains(filterSearch.ToLower()));

            return View(filter);
        }
    }
}
