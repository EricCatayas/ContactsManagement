using ContactsManagement.Web.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ContactsManagement.Core.DTO.ContactsManager;

namespace ContactsManagement.Web.Filters.ActionFilters
{
    public class PersonsListActionFilter : IActionFilter 
    {
        //inject logger
        private readonly ILogger<PersonsListActionFilter> _logger;        

        public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
        {
            _logger = logger;            
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("{FilterName}.{MethodName} method", nameof(PersonsListActionFilter), nameof(OnActionExecuted));

            PersonsController personController = (PersonsController)context.Controller;

            IDictionary<string, object?>? ActionArgs = (IDictionary<string, object?>?)context.HttpContext.Items["arguments"];

            personController.ViewBag.SearchProperties = new Dictionary<string, string>()
            {
                { "Name", nameof(PersonResponse.PersonName) },
                { "Email", nameof(PersonResponse.Email) },
                { "Job Title", nameof(PersonResponse.JobTitle) },
                { "Tag", nameof(PersonResponse.Tag.TagName) },
                { "Company Name", nameof(PersonResponse.CompanyName) },
            };            

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
            /*if (ActionArgs.ContainsKey("error"))
            {
                var errors = ActionArgs["error"];
                personController.ViewBag.Errors = errors
            }*/
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items["arguments"] = context.ActionArguments;  // <-- passing args to OnActionExecuted -- HttpContext is accessible everywhere
            _logger.LogInformation("{FilterName}.{MethodName}", nameof(PersonsListActionFilter), nameof(OnActionExecuting));

        }
    }
}
