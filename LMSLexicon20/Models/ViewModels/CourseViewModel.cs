using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Course Name")]
        public Course Course { get; set; }

      

        [Display(Name = "Description ")]
        public string Description { get; set; }

        [Display(Name = "Start date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        public Module Module { get; set; }

      
    }
}
