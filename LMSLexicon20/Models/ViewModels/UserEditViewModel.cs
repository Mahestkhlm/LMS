﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models.ViewModels
{
    public class UserEditViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }

        [Display(Name = "Efternamn")]
        public string LastName { get; set; }
        public string Email { get; set; }

        [Display(Name = "Telefonnummer")]
        public string PhoneNumber { get; set; }
        public int CourseId { get; set; }
    }
}