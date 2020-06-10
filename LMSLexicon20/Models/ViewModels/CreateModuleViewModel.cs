using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models.ViewModels
{
    public class CreateModuleViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Module Name")]
        public string  Name { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public string Description { get; set; }
        

        public int CourseId { get; set; }
        public Course Course { get; set; }
        public ICollection<Document> Documents { get; set; }
        public ICollection<Activity> Activities { get; set; }
    }
}
