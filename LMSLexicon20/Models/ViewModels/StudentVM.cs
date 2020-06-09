using System.ComponentModel.DataAnnotations;

namespace LMSLexicon20.Models.ViewModels
{
    public class StudentVM
    {
        public string Id { get; set; }

        [Display(Name = "Namn")]
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}

