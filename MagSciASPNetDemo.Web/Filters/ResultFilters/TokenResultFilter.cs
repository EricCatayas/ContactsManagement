using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManagement.Web.Filters.ResultFilters
{
    /// <summary>
    /// For demo purposes only
    /// </summary>
    public class TokenResultFilter : ActionFilterAttribute, IResultFilter
    {
        private readonly ILogger<TokenResultFilter> _logger;
        private readonly string _authKey;
        private readonly string _authValue;
        public TokenResultFilter(ILogger<TokenResultFilter> logger, string cookieKey, string cookieValue)
        {
            _logger = logger;
            _authKey = cookieKey;
            _authValue = cookieValue;
        }
        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Cookies.Append(_authKey, _authValue);
        }
    }
}
