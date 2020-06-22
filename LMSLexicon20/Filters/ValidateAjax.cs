using LMSLexicon20.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LMSLexicon20.Filters
{
    public class ValidateAjax : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.IsAjax())
            {
                if (!context.ModelState.IsValid)
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
        }
    }

}
