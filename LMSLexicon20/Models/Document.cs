using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models
{
    public class Document
    {
        public int Id { get; set; }

        [StringLength(30)]
        [Required]
        public string Name { get; set; }

        [StringLength(200)]
        [Required]
        public string Description { get; set; }

        [Display(Name = " Date")]
        public DateTime Date { get; set; }
        public string Path { get; set; }

        //Foreign Key
        public string UserId { get; set; }
        public int? CourseId { get; set; }

        public int? ModuleId { get; set; }

        public int? ActivityId { get; set; }

        //Navigation Prop
        public Course Course { get; set; }
        public Module Module { get; set; }
        public Activity Activity { get; set; }
        public User User { get; set; }
    }
    
}
