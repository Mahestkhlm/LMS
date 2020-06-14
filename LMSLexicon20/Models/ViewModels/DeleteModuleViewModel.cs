using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models.ViewModels
{
    public class DeleteModuleViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Kurs")]
        public Course Course { get; set; }
       
        [Display(Name = "Namn")]
        public string Name { get; set; }
       
        [Display(Name = "Startdatum")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
     
        [Display(Name = "Slutdatum")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
    }
}
