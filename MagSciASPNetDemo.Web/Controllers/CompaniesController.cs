using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.DTO.CompaniesManagement;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.ServiceContracts.CompaniesManagement;
using ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml.Drawing.Chart;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ContactsManagement.Web.Controllers
{
    [Route("[controller]")]
    public class CompaniesController : Controller
    {
        private readonly ICompanyAdderService _companiesAdderService;
        private readonly ICompanyGetterService _companiesGetterService;
        private readonly ICompanyUpdaterService _companiesUpdaterService;
        private readonly ICompanyDeleterService _companiesDeleterService;
        private readonly IPersonsGetterService _personsGetterService;

        private List<string> CompanyIndustries;
        public CompaniesController(ICompanyAdderService companyAdderService, ICompanyGetterService companyGetterService, ICompanyUpdaterService companiesUpdaterService, ICompanyDeleterService companyDeleterService, IPersonsGetterService personsGetterService)
        {
            _companiesAdderService = companyAdderService;
            _companiesGetterService = companyGetterService;
            _companiesUpdaterService = companiesUpdaterService;
            _companiesDeleterService = companyDeleterService;
            _personsGetterService = personsGetterService;
            CompanyIndustries = new List<string>()
            {
                "Agriculture and farming",
                "Apparel and textiles",
                "Automotive and transportation",
                "Banking and finance",
                "Biotech and pharmaceuticals",
                "Business services",
                "Construction and engineering",
                "Consumer goods and services",
                "Education and training",
                "Energy and utilities",
                "Entertainment and media",
                "Food and beverage",
                "Government and public administration",
                "Healthcare and medical",
                "Hospitality and tourism",
                "Information technology and software",
                "Insurance and risk management",
                "Legal services",
                "Manufacturing and production",
                "Marketing and advertising",
                "Nonprofit and social services",
                "Real estate and property management",
                "Retail and ecommerce",
                "Telecommunications",
                "Transportation and logistics"
            };
            
        }
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Index(List<string>? errors = null)
        {
            ViewBag.Errors = errors;
            List<CompanyResponse>? companies = await _companiesGetterService.GetAllCompanies();
            return View(companies);
        }
        //Model Validaiton Filter
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create(CompanyAddRequest companyAddRequest)
        {
            List<PersonResponse> persons = await _personsGetterService.GetAllPersons();

            if(ModelState.IsValid)
            {
                CompanyResponse companyResponse = await _companiesAdderService.AddCompany(companyAddRequest);
                ViewBag.Success = "Company has been added";
                return View("Details", companyResponse);
            }
            else
            {
                ViewBag.CompanyIndustries = CompanyIndustries;
                ViewBag.Persons = persons.ToList();
                ViewBag.Errors = ModelState.Values.SelectMany(V => V.Errors).Select(err => err.ErrorMessage).ToList();
                return View();
            }
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Create()
        {
            ViewBag.CompanyIndustries = CompanyIndustries;
            ViewBag.Persons = await _personsGetterService.GetAllPersons();

            return View();
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Details(int companyId)
        {
            CompanyResponse? company = await _companiesGetterService.GetCompanyById(companyId);
            if (company == null)
                return RedirectToAction("Index", new { errors = new List<string>() { "Company does not exist" } });

            return View(company);
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Edit(int companyId)
        {
            CompanyResponse? company = await _companiesGetterService.GetCompanyById(companyId);
            if (company == null)
                return RedirectToAction("Index", new { errors = new List<string>() { "Company does not exist" } });

            ViewBag.CompanyIndustries = CompanyIndustries;
            ViewBag.Employees = company.Employees?.Select(p => p.PersonId).ToList();
            ViewBag.Persons = await _personsGetterService.GetAllPersons();
            return View(company.ToCompanyUpdateRequest());
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Edit(CompanyUpdateRequest companyUpdateRequest)
        {

            if (ModelState.IsValid)
            {
                CompanyResponse companyResponse = await _companiesUpdaterService.UpdateCompany(companyUpdateRequest);
                ViewBag.Success = "Successfully updated company";
                return RedirectToAction("Details", new { companyId = companyResponse.CompanyId });
            }
            else
            {
                ViewBag.CompanyIndustries = CompanyIndustries;
                ViewBag.Employees = companyUpdateRequest.Employees;
                ViewBag.Persons = await _personsGetterService.GetAllPersons();
                ViewBag.Errors = ModelState.Values.SelectMany(V => V.Errors).Select(err => err.ErrorMessage).ToList();
                return View();
            }
        }
        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> Delete(int companyId)
        {
            bool isDeleted = await _companiesDeleterService.DeleteCompany(companyId);

            if (isDeleted)
            {
                return StatusCode(200);
            }
            else
            {
                return StatusCode(500);
            }
        }
    }
}
