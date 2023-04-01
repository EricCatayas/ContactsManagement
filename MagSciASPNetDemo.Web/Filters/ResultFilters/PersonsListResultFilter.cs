using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManagement.Web.Filters.ResultFilters
{
    public class PersonsListResultFilter : IAsyncResultFilter
    {
        public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}
