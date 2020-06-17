using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models.ViewModels
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Fyll i fältet")]
        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Fyll i fältet")]
        [Display(Name = "Efternamn")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Fyll i fältet")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Fyll i fältet")]
        [Display(Name = "Telefonnummer")]
        public string PhoneNumber { get; set; }
        public DateTime RegDate => DateTime.Now;

        [Display(Name = "Kurs-ID")]
        [Remote(action: "DoesCourseExist", controller: "Courses", HttpMethod = "POST", ErrorMessage = "Kursen finns inte")]

        public int? CourseId { get; set; }

    }
}
