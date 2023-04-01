using ContactsManagement.Web.Filters.CustomFilters;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManagement.Web.Filters.AlwaysRunResultFilter
{
    public class AlwaysRunResultFilter : IAlwaysRunResultFilter
    {
        private readonly ILogger<AlwaysRunResultFilter> _logger;
        public AlwaysRunResultFilter(ILogger<AlwaysRunResultFilter> logger)
        {
            _logger = logger;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Filters.OfType<SkipFilter>().Any())
            {
                _logger.LogInformation("SkipFilter detected, cancelling {FilterName}.{MethodName}", nameof(AlwaysRunResultFilter), nameof(OnResultExecuting));
                return;
            }
        }
    }
}