using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManagement.Web.Filters.ExceptionFilters
{
    public class HandleExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HandleExceptionFilter> _logger;
        private readonly IHostEnvironment _hostEnvironment;
        public HandleExceptionFilter(ILogger<HandleExceptionFilter> logger, IHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }


        public void OnException(ExceptionContext context)
        {
            _logger.LogError("Exception Filter {FilterName}.{MethodName}\n{ExcetionType}\n{ExceptionMessage}", nameof(HandleExceptionFilter), nameof(OnException), context.Exception.GetType().ToString(), context.Exception.Message);

            if (_hostEnvironment.IsDevelopment())
            {
                context.Result = new ContentResult()
                {
                    Content = context.Exception.Message,
                    StatusCode = 500
                };
            }
            else
            {
                // context.Result = new RedirectToActionResult("Index", "Person", new {error = context.Exception.Message});
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
