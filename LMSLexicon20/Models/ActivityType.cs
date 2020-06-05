using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models
{
    public class ActivityType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool RequireDocument { get; set; }


        //Navigation Prop
        public ICollection<Activity> Activities { get; set; }
    }
}
