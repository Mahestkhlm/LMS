using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSLexicon20.Services
{
    public interface ITeacherChoiceDropdown
    {
        Task<IEnumerable<SelectListItem>> GetTeacherList();
    }
}