using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models
{
    public class Module
    {
        public int Id { get; set; }


        [StringLength(30)]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [StringLength(200)]
        [Required]
        public string Description { get; set; }

        //Foreign Key
        public int CourseId { get; set; }

        //Navigation Prop
        public Course Course { get; set; }
        public ICollection<Activity> Activities { get; set; }
        public ICollection<Document> Documents { get; set; }

    }
}
