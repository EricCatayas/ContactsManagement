using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManagement.Web.Filters.AuthorizationFilter
{
    public class TokenAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Cookies.ContainsKey("Auth-Key") &&
               context.HttpContext.Request.Cookies["Auth-Key"] != "A-100")
            {
                context.Result = new UnauthorizedResult();  //Unathorized
            }
        }
    }
}
