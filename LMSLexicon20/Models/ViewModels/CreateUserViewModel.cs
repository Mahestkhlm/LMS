using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models.ViewModels
{
    public class CreateUserViewModel
    {
        [Required]
        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Efternamn")]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Telefonnummer")]
        public string PhoneNumber { get; set; }
        public DateTime RegDate => DateTime.Now;

        [Display(Name = "Kurs-ID")]
        public int? CourseId { get; set; }

    }
}
