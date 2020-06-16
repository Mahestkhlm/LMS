using LMSLexicon20.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.ViewModels
{
    public class CourseDetailVM
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Kursnamn")]
        public string Name { get; set; }


        [Required]
        [Display(Name = "Startdatum")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Slutdatum")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [StringLength(200)]
        [Display(Name = "Kursbeskrivning")]
        public string Description { get; set; }

        [Display(Name = "Lärare")]
        public User Teacher { get; set; }

        public ICollection<ModuleDetailVM> ModuleDetailVM { get; set; }


    }
}
