using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models.ViewModels
{
    public class UserListViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Namn")]
        public string FullName { get; set; }
        public string Email { get; set; }
        [Display(Name = "Kurs")]
        public string CourseName { get; set; }
    }
}
