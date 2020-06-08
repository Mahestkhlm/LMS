using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public bool HasDeadline { get; set; }

        //Foreign Key
        public int ModuleId { get; set; }
        public int ActivityTypeId { get; set; }

        //Navigation Prop
        public Module Module { get; set; }
        public ActivityType ActivityType { get; set; }
        public ICollection<Document> Documents { get; set; }

    }
}
