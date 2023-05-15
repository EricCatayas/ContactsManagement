using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManagement.Web.Filters.ExceptionFilters
{
    public class RedirectToIndexExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

            if (controllerActionDescriptor != null)
            {
                string controllerName = controllerActionDescriptor.ControllerName;
                string actionMethod = controllerActionDescriptor.ActionName;

                if (actionMethod == "Index")
                {
                    context.Result = new RedirectToActionResult("Error", "Home", new { });
                } else
                {
                    context.Result = new RedirectToActionResult("Index", controllerName, new { errors = new List<string>() { context.Exception.Message } });
                }
            }
            else
            {
                context.Result = new RedirectToActionResult("Error", "Home", new { });
            }
        }
    }
}
