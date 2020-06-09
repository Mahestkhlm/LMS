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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegDate { get; set; }

        //Foreign Key
        public int? CourseId { get; set; }

        //Navigation Prop
        public Course Course { get; set; }
        public ICollection<Document> Documents { get; set; }


    }
}
