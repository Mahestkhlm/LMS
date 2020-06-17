using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models.ViewModels
{
    public class AddTeacherToCourseViewModel
    {
        [Required]
        public string TeacherId { get; set; }
        public int? Id { get; set; }
    }
}
