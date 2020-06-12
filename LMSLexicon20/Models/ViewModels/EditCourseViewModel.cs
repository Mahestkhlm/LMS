using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models.ViewModels
{
    public class EditCourseViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Lärare")]
        public User Teacher { get; set; }

        [Required(ErrorMessage = "Fyll i fältet")]
        [Display(Name = "Kursnamn")]
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
