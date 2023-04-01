using ContactsManagement.Web.Filters.ResultFilters;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManagement.Web.Filters.FactoryFilters
{
    /// <summary>
    /// N: ActionFilterAttribute cannot implement DI
    ///     FactoryFilter = Attribute + DI
    /// </summary>
    public class TokenResultFactoryFilter : Attribute, IFilterFactory
    {
        public bool IsReusable { get; }
        private string? _authKey { get; set; }
        private string? _authValue { get; set; }
        public TokenResultFactoryFilter(bool isReusable, string cookieKey, string cookieValue)
        {
            IsReusable = isReusable;
            _authKey = cookieKey;
            _authValue = cookieValue;
        }
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var _logger = serviceProvider.GetRequiredService<ILogger<TokenResultFilter>>();
            TokenResultFilter tokenResultFilter = new TokenResultFilter(_logger, _authKey, _authValue);

            return tokenResultFilter;
        }
    }
}
