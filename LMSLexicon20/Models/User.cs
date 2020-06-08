using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models
{
    public class User : IdentityUser
    {
        //  public int Id { get; set; }

        [StringLength(30)]
        [Required]
        public string FirstName { get; set; }

        [StringLength(30)]
        [Required]
        public string LastName { get; set; }

        [Display(Name = " Reg Date")]
        public DateTime RegDate { get; set; }

        //Foreign Key
        public int? CourseId { get; set; }


        //Navigation Prop
        public Course Course { get; set; }
        public ICollection<Document> Documents { get; set; }


    }
}
