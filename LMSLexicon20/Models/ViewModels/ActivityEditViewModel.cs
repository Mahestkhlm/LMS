using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models.ViewModels
{
    public class ActivityEditViewModel
    {

        public int Id { get; set; }
        //public string Type { get; set; }
        [Required(ErrorMessage = "Du måste ange ett namn")]
        [MaxLength(30, ErrorMessage = "Max 30 tecken")]
        [Display(Name = "Namn")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Fyll i fältet")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Starttid")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Du måste ange en sluttid")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Sluttid")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Modul")]
        public string Description { get; set; }
        public string HasDeadline { get; set; }

        public int ModuleId { get; set; }
        [Display(Name = "Aktivitet")]
        public int ActivityTypeId { get; set; }
    }
}
