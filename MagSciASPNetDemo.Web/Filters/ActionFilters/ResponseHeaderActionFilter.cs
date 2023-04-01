using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManagement.Web.Filters.ActionFilters
{
    /// <summary>
    /// This is not limited to IActionMethods, Controllers and Program.cs can invoke this Filter IF the corresponding ActionMethod is called
    /// </summary>
    public class ResponseHeaderActionFilter : ActionFilterAttribute
    {
        // private readonly ILogger<ResponseHeaderActionFilter> _logger; No DI support
        private readonly string Key;
        private readonly string Value;
        public ResponseHeaderActionFilter(string key, string value, int order)
        {
            Key = key;
            Value = value;
            Order = order;
        }

        // await next() // calls the next subsequent filter
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Response.Headers[Key] = Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
