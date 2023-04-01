using ContactsManagement.Core.ServiceContracts.ContactsManager;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManagement.Web.Controllers
{
    public class CountriesController : Controller
    {
        private readonly ICountriesService _countriesService;
        public CountriesController(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult UploadExcelFile()
        {
            return View();
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> UploadExcelFile(IFormFile excelFile)
        {
            if(excelFile == null)
            {
                ViewBag.Errors = "Insert a file to upload";
                return View();
            }
            if (!Path.GetExtension(excelFile.FileName).Equals("xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.Errors = "Unsupported file. File extension must be an xlsx file";
                return View();
            }
            int result = await _countriesService.UploadCountriesFromExcelFile(excelFile);
            if (result < 0) {
                ViewBag.Success = $"{result} countries have been successfully uploaded!";
                return View("/");
            } else
            {
                ViewBag.Erros = "Something went wrong. Please try again";
                return View();
            }
        }
    }
}
