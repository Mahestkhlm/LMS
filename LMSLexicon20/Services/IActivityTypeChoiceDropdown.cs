using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace LMSLexicon20.Services
{
    public interface IActivityTypeChoiceDropdown
    {
        IEnumerable<SelectListItem> GetSelectList();
    }
}