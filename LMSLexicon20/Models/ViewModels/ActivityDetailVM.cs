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
        public bool Expanded { get; set; } = false;

        public bool StartDateToEarly { get; set; }
        public bool StartDateToLate { get; set; }
        public bool EndDateToEarly { get; set; }
        public bool EndDateToLate { get; set; }
        public bool StartDateOverlap { get; set; }
        public bool EndDateOverlap { get; set; }

        //Foreign Key
        public int ModuleId { get; set; }

        public ActivityTypeWM ActivityTypeWM { get; set; }

    }
}