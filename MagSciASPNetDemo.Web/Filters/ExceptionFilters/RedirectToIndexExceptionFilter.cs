using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManagement.Web.Filters.ExceptionFilters
{
    public class RedirectToIndexExceptionFilter : IAsyncExceptionFilter
    {
        public readonly ILogger<RedirectToIndexExceptionFilter> _logger;
        public RedirectToIndexExceptionFilter(ILogger<RedirectToIndexExceptionFilter> logger)
        {
            _logger = logger;
        }
        public Task OnExceptionAsync(ExceptionContext context)
        {
            _logger.LogError("Exception Filter {FilterName}.{MethodName}\n{ExceptionType}\n{ExceptionMessage}", nameof(RedirectToIndexExceptionFilter), nameof(OnExceptionAsync), context.Exception.GetType().ToString(), context.Exception.Message);
            context.Result = new RedirectToActionResult("Index", "Home", new { });
            return Task.CompletedTask;
        }
    }
}
