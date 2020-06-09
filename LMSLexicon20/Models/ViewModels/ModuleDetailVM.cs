using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMSLexicon20.ViewModels
{
    public class ModuleDetailVM
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

        public bool OpenModule { get; set; }


        //Foreign Key
        public int CourseId { get; set; }

        public ICollection<ActivityDetailVM> ActivityDetailVM { get; set; }
    }
}