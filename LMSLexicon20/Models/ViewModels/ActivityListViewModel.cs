using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models.ViewModels
{
    public class ActivityListViewModel
    {

         public int Id { get; set; }
        //public string Type { get; set; }
        [Display(Name = "Namn")]
        public string Name { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Starttid")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")] 
        public DateTime StartDate { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Sluttid")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Aktivitetbeskrivning")]
        public String Description { get; set; }
        public bool HasDeadline { get; set; }
        [Display(Name = "Modul")]
        public int ModuleId { get; set; }

        [Display(Name = "Aktivitet")]
        public int ActivityTypeId { get; set; }
        
        [Display(Name = "Module Namn")]
        public Module Module { get; set; }
        [Display(Name = "Aktivitet")]
        public ActivityType  ActivityType { get; set; }
    }
}
