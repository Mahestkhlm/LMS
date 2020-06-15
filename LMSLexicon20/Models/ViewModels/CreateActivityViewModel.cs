using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models.ViewModels
{
    public class CreateActivityViewModel
    {

       
        [Required(ErrorMessage = "Fyll i fältet")]
        [Display(Name = "Aktivitet namn")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Välj en datum")]
        [Display(Name = "Aktivitet start")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Välj en datum")]
        [Display(Name = "Aktivitet slut")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Aktivitetbeskrivning")]
        public string Description { get; set; }

        public bool HasDeadline { get; set; }

        public int ModuleId { get; set; }
        [Display(Name = "Aktivitet")]
        public int ActivityTypeId { get; set; }
    }
}
