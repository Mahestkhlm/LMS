using LMSLexicon20.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Services
{
    public class ActivityTypeChoiceDropdown : IActivityTypeChoiceDropdown
    {
        private readonly ApplicationDbContext context;

        public ActivityTypeChoiceDropdown(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<SelectListItem> GetSelectList()
        {
            return context.ActivityTypes.Select(e => new SelectListItem { Text = e.Name, Value = e.Id.ToString() });
        }
    }
}
