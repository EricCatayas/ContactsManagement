using ContactsManagement.Web.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using Microsoft.AspNetCore.Http;
using ContactsManagement.Core.Enums.ContactsManager;
using ContactsManagement.Core.Helpers;

namespace ContactsManagement.Web.Filters.ActionFilters
{
    public class PersonsListActionFilter : IActionFilter 
    {
        private readonly ILogger<PersonsListActionFilter> _logger;
        private const string contactDisplayTypeKey = "ContactsDisplayType";

        public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
        {
            _logger = logger;            
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("{FilterName}.{MethodName} method", nameof(PersonsListActionFilter), nameof(OnActionExecuted));

            PersonsController personController = (PersonsController)context.Controller;            

            IDictionary<string, object?>? ActionArgs = (IDictionary<string, object?>?)context.HttpContext.Items["arguments"];

            if (ActionArgs.ContainsKey("searchProperty"))
            {
                personController.ViewBag.searchProperty = ActionArgs["searchProperty"]?.ToString();
            }
            if (ActionArgs.ContainsKey("searchString"))
            {
                personController.ViewBag.searchString = ActionArgs["searchString"]?.ToString();
            }
            if (ActionArgs.ContainsKey("sortProperty"))
            {
                personController.ViewBag.sortProperty = ActionArgs["sortProperty"]?.ToString();
            }
            if (ActionArgs.ContainsKey("sortOrder"))
            {
                personController.ViewBag.sortOrder = ActionArgs["sortOrder"]?.ToString();
            }
            if (ActionArgs.ContainsKey("groupId"))
            {
                personController.ViewBag.groupId = Convert.ToInt32(ActionArgs["groupId"]);
            }

            // Display type Cookie
            string? displayContactsType;
            if (ActionArgs.ContainsKey("displayType"))
            {
                displayContactsType = ActionArgs["displayType"]?.ToString();

                CookieOptions cookieOptions = new CookieOptions()
                {
                    Expires = DateTime.Now.AddDays(30),
                    Secure = true, // Set to true if using HTTPS
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict
                };
                context.HttpContext.Response.Cookies.Append(contactDisplayTypeKey, displayContactsType, cookieOptions);
            }
            else
            {
                displayContactsType = context.HttpContext.Request.Cookies[contactDisplayTypeKey];
            }

            if (displayContactsType == null)
            {
                CookieOptions cookieOptions = new CookieOptions()
                {
                    Expires = DateTime.Now.AddDays(30),
                    HttpOnly = true, 
                    SameSite = SameSiteMode.Strict 
                };
                context.HttpContext.Response.Cookies.Append(contactDisplayTypeKey, ContactsDisplayType.Profile.ToString(), cookieOptions);
            }

            ContactsDisplayType displayType = ContactsDisplayHelper.GetDisplayType(displayContactsType);

            personController.ViewBag.SearchProperties = ContactsDisplayHelper.GetDisplayProperties(displayType);
            personController.ViewBag.ContactsDisplayType = displayType;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items["arguments"] = context.ActionArguments;  // <-- passing args to OnActionExecuted -- HttpContext is accessible everywhere
            _logger.LogInformation("{FilterName}.{MethodName}", nameof(PersonsListActionFilter), nameof(OnActionExecuting));

        }
    }
}
