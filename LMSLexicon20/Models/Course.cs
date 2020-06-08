using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models
{
    public class Course
    {

        public int Id { get; set; }


        [StringLength(100)]
        [Required]
        public string Name { get; set; } //Course Name

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [StringLength(200)]
        [Required]
        public string Description { get; set; }


        //nav prop
        public ICollection<User> Users { get; set; }
        public ICollection<Module> Modules { get; set; }
        public ICollection<Document> Documents { get; set; }

    }
}
