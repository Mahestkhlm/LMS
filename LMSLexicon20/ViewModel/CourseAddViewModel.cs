using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.ViewModel
{
    public class CourseAddViewModel
    {

        public class CourseViewModel
        {
            //public int Id { get; set; }

            [Required]
            [Display(Name = "Course Name")]
            public string Name { get; set; } //Course Name


            [Required]
            [Display(Name = "Start Date")]
            [DataType(DataType.Date)]
            public DateTime StartDate { get; set; }

            [Required]
            [Display(Name = "End Date")]
            [DataType(DataType.Date)]
            public DateTime EndDate { get; set; }

            [StringLength(200)]
            public string Description { get; set; }

            
        }

    }
}
