using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models
{
    public class ActivityTypeWM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool RequireDocument { get; set; }
    }
}
