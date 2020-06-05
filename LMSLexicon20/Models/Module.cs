using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models
{
    public class Module
    {
        public int Id { get; set; }


        public string Name { get; set; } 

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }

        //Foreign Key
        public int CourseId { get; set; }

        //Navigation Prop
        public Course Course { get; set; }
        public ICollection<Activity> Activities { get; set; }
        public ICollection<Document> Documents { get; set; }

    }
}
