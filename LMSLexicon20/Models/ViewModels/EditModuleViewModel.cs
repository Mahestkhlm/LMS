using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models.ViewModels
{
    public class EditModuleViewModel
    {
        public int CourseId { get; set; }
        [Required]
        [Display(Name = "Namn")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Startdatum")]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "Slutdatum")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
    }
}
