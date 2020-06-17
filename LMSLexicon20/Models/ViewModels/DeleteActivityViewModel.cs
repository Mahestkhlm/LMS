using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models.ViewModels
{
    public class DeleteActivityViewModel
    {

        public int Id { get; set; }
        [Display(Name = "Namn")]
        public string Name { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Starttid")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime StartDate { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Sluttid")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime EndDate { get; set; }


        [Display(Name = "Aktivitetbeskrivning")]
        public string Description { get; set; }

        [Display(Name = "Deadline")]
        public bool HasDeadline { get; set; }

        [Display(Name = "Tillhör modul")]
        public Module Module { get; set; }
        [Display(Name = "Aktivitetstyp")]
        public ActivityType ActivityType { get; set; }
    }
}
