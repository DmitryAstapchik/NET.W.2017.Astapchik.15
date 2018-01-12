using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PL.Web.Filters
{
    public class ConfirmCreateAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var viewData = new ViewDataDictionary { Model = filterContext.ActionParameters["account"] };
            if (filterContext.Controller.ViewData.ModelState.IsValid)
            {
                filterContext.Result = new ViewResult { ViewName = "create", ViewData = filterContext.Controller.ViewData };
            }
        }
    }
}