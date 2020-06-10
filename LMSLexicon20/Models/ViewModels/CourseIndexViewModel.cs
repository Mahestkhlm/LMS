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

        [Display(Name = "Course Name")]
        public string Name { get; set; }

        [Display(Name = "Start date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Description ")]
        public string Description { get; set; }
        //public Module Module { get; set; }
       // public Course Course { get; set; }

    }
}
