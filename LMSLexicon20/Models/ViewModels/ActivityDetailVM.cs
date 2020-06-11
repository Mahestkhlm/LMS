using LMSLexicon20.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace LMSLexicon20.ViewModels
{
    public class ActivityDetailVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Kursnamn")]
        public string Name { get; set; }


        [Required]
        [Display(Name = "Startdatum")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Slutdatum")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public bool HasDeadline { get; set; }

        public bool ExpandedActivity { get; set; } = false;

        //Foreign Key
        public int ModuleId { get; set; }

        public ActivityTypeWM ActivityTypeWM { get; set; }

    }
}