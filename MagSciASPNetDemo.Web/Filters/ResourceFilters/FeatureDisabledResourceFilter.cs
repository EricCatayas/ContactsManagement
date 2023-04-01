using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManagement.Web.Filters.ResourceFilters
{
    public class FeatureDisabledResourceFilter : IAsyncResourceFilter
    {
        private readonly bool _isDisabled;
        public FeatureDisabledResourceFilter(bool isDisabled)
        {
            _isDisabled = isDisabled;
        }
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            if (_isDisabled)
            {
                context.Result = new NotFoundResult();
                return;
            }
            else
            {
                await next();
            }
        }
    }
}
