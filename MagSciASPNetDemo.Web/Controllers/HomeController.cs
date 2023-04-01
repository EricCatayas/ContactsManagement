using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManagement.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller //TODO : Redirect to Error View Exception filter
    {
        [Route("~/")]
        [Route("[action]")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [Route("[action]")]
        public IActionResult Error()
        {
            //Features: an additional info provider 

            IExceptionHandlerPathFeature? exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            
            if(exceptionHandlerPathFeature != null && exceptionHandlerPathFeature.Error != null){
                ViewBag.ErrorMessage = exceptionHandlerPathFeature.Error.Message;
            } else
            {
                ViewBag.ErrorMessage = "An error occurred. Please try again";
            }
            
            return View();
        }
    }
}
