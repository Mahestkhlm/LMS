using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models.ViewModels
{
    public class StudentIndexViewModel
    {
        //public string Id { get; set; }
        [Display(Name = "Namn")]
        public string FullName { get; set; }
        //public string Email { get; set; }

        //[Display(Name = "Telefonnummer")]
        //public string PhoneNumber { get; set; }
        [Display(Name = "Kurs")]
        public Course Course { get; set; }
        public Activity Activity { get; set; }
        public ICollection <Activity>Activities { get; set; }
        public ICollection<Document> Documents { get; set; }
    }
}
