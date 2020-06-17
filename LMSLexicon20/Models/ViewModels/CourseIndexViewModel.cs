using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models.ViewModels
{
    public class CourseIndexViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Kursnamn")]
        public string Name { get; set; }

        [Display(Name = "Startdatum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Slutdatum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Kursbeskrivning")]
        public string Description { get; set; }
        //public Module Module { get; set; }
       // public Course Course { get; set; }

    }
}
