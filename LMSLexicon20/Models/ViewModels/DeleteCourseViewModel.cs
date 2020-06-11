using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models.ViewModels
{
    public class DeleteCourseViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Kursnamn")]
        public string Name { get; set; }

        [Display(Name = "Kursstart")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Kursslut")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Kursbeskrivning")]
        public string Description { get; set; }

        [Display(Name = "Lärare")]
        public User Teacher { get; set; }

        [Display(Name = "Studenter")]
        public ICollection<User> Students { get; set; }
    }
}
