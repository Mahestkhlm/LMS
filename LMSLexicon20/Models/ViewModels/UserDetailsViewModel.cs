using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models.ViewModels
{
    public class UserDetailsViewModel
    {
        [Display(Name = "Roll")]
        public string Role { get; set; }

        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }

        [Display(Name = "Efternamn")]
        public string LastName { get; set; }

        [Display(Name = "Registreringsdatum")]
        public DateTime RegDate { get; set; }

        [Display(Name = "Kurs")]
        public Course Course { get; set; }
    }
}
