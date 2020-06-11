using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models.ViewModels
{
    public class CreateCourseViewModel
    {
        [Required(ErrorMessage = "Fyll i fältet")]
        [Display(Name = "Kursnamn")]
        [Remote(action: "IsNameUnique", controller: "Courses", ErrorMessage = "Det finns redan en kurs med denna namn")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Välj en datum")]
        [Display(Name = "Kursstart")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Välj en datum")]
        [Display(Name = "Kursslut")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Kursbeskrivning")]
        public string Description { get; set; }
    }
}
