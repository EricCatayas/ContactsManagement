using ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.Enums.ContactsManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager;
using ContactsManagement.Web.Filters.ActionFilters;
using ContactsManagement.Web.Filters.AlwaysRunResultFilter;
using ContactsManagement.Web.Filters.AuthorizationFilter;
using ContactsManagement.Web.Filters.ExceptionFilters;
using ContactsManagement.Web.Filters.FactoryFilters;
using ContactsManagement.Web.Filters.ResourceFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactGroupsServices;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;

namespace ContactsManagement.Web.Controllers
{
    [Route("persons")]
    [TypeFilter(typeof(AlwaysRunResultFilter))]
    [ResponseHeaderActionFilter("Controller-Key", "In-PersonsController",3)]
    public class PersonsController : Controller
    {
        private readonly IPersonsGetterService _personGetterService;
        private readonly IPersonsAdderService _personAdderService;
        private readonly IPersonsDeleterService _personsDeleterService;
        private readonly IPersonsSorterService _personsSorterService;
        private readonly IPersonsUpdaterService _personsUpdaterService;
        private readonly ICountriesService _countriesService;
        private readonly IContactGroupsGetterService _contactGroupsGetterService;
        private readonly ILogger<PersonsController> _logger;
        public PersonsController(ILogger<PersonsController> logger, IPersonsGetterService personsGetterService, IPersonsAdderService personsAdderService, IPersonsDeleterService personsDeleterService, IPersonsSorterService personsSorterService, IPersonsUpdaterService personsUpdaterService, ICountriesService countriesService, IContactGroupsGetterService contactGroupsGetterService)
        {
            _logger = logger;
            _personGetterService = personsGetterService;
            _personAdderService = personsAdderService;
            _personsSorterService = personsSorterService;
            _personsUpdaterService = personsUpdaterService;
            _personsDeleterService = personsDeleterService;
            _countriesService = countriesService;
            _contactGroupsGetterService = contactGroupsGetterService;
        }        
        [Route("[action]")]
        [AllowAnonymous]
        [TypeFilter(typeof(PersonsListActionFilter))]
        [ResponseHeaderActionFilter("Action-Key", "In-Index-Method", 1)]              
        [TokenResultFactoryFilter(true, "Auth-Key", "A-100")]
        public async Task<IActionResult> Index(string? searchProperty, string? searchString, int? groupId, string sortProperty = "Name", string sortOrder = "ASC", List<string>? error = null)
        {
            _logger.LogDebug($"searchProperty: {searchProperty}, searchString: {searchString}, sortProperty: {sortProperty}, sortOrder: {sortOrder}");
            // ViewData handled in PersonsListActionFilter     
            ViewBag.Errors = error;
            List<PersonResponse>? personList = await _personGetterService.GetFilteredPersons(searchProperty, searchString);
            if(groupId != null)
            {
                personList = personList.Where(person => person.ContactGroups.Any(group => group.GroupId == groupId)).ToList();
            }
            List<ContactGroupResponse>? contactGroups = await _contactGroupsGetterService.GetAllContactGroups();
            if (contactGroups != null)
            {
                Dictionary<string, int> contactGroupsFilter = new Dictionary<string, int>() { };
                foreach (ContactGroupResponse contactGroup in contactGroups)
                {
                    contactGroupsFilter.Add(contactGroup.GroupName, contactGroup.GroupId);
                }
                ViewBag.ContactGroupsFilter = contactGroupsFilter;
            }
            //Sorting
            switch (sortOrder)
            {
                case "ASC":
                    personList = await _personsSorterService.GetSortedPersons(personList, sortProperty, SortOrderOptions.ASC);
                    break;
                case "DESC":
                    personList = await _personsSorterService.GetSortedPersons(personList, sortProperty, SortOrderOptions.DESC);
                    break;
            }
            return View(personList);
        }
        [HttpGet]
        [Route("create")]
        [TypeFilter(typeof(PersonCreateAndEditActionFilter))]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [Route("create")]
        [TypeFilter(typeof(PersonCreateAndEditActionFilter))]
        public async Task<IActionResult> Create(PersonAddRequest? personAddRequest)
        {
            _ = await _personAdderService.AddPerson(personAddRequest);
            ViewBag.Success = "Person has been successfully added!";
            return View();
        }
        [HttpPost]
        [Route("[action]/{personId}")]
        [TypeFilter(typeof(PersonCreateAndEditActionFilter))]       
        [ServiceFilter(typeof(RedirectToIndexExceptionFilter))] // Can't supply filters
        public async Task<IActionResult> Edit(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? updatedPerson = await _personsUpdaterService.UpdatePerson(personUpdateRequest);              
                
            PersonUpdateRequest personToUpdate = updatedPerson.ToPersonUpdateRequest(); //Check ToPersonUpdateRequest-- catch block not caught in Service
            ViewBag.Success = "Person has been successfully updated!";
            return View(personToUpdate);            
        }
        [HttpGet]
        [Route("[action]/{personId}")]
        [TypeFilter(typeof(PersonCreateAndEditActionFilter))]        
        [TypeFilter(typeof(HandleExceptionFilter))]
        public async Task<IActionResult> Edit(Guid? personId, List<string>? error = null)
        {
            PersonResponse? matchingPerson = await _personGetterService.GetPersonById(personId);
            if (matchingPerson == null)
                return RedirectToAction("Index");

            ViewBag.Errors = error;
            ViewBag.Countries = await LoadCountrySelectItems();
            PersonUpdateRequest personToUpdate = matchingPerson.ToPersonUpdateRequest();
            return View(personToUpdate);
        }
        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> Delete(Guid personId)
        {
            bool isDeleted = await _personsDeleterService.DeletePerson(personId);
            if (isDeleted)
            {
                return StatusCode(200);
            }
            else
            {
                return StatusCode(500);
            }
        }
        [Route("[action]")]
        public async Task<IActionResult> PersonsPDF()
        {
            List<PersonResponse> allPersons = await _personGetterService.GetAllPersons();
            if (allPersons.IsNullOrEmpty())
                return RedirectToAction("Index", new { error = "Table is empty" });

            //Rotativa
            return new ViewAsPdf("PersonsPDF", allPersons, ViewData)
            {
                PageMargins = new Margins()
                {
                    Top = 20,
                    Right = 20,
                    Left = 20,
                    Bottom = 20
                },
                PageOrientation = Orientation.Landscape
            };
        }        
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> PersonsCSV()
        {
            MemoryStream memoryStream = await _personGetterService.GetPersonsCSV();
            return File(memoryStream, "application/octet-stream", "Persons.csv");
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> PersonsExcel()
        {
            MemoryStream memoryStream = await _personGetterService.GetPersonsEXCEL();
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Persons.xlsx");
        }
        /*  ------------ TODO --------------- */
        public async Task<List<SelectListItem>> LoadCountrySelectItems()
        {
            List<CountryResponse> countries = await _countriesService.GetAllCountries();
            return countries.Select(country => new SelectListItem()
            {
                Text = country.CountryName,
                Value = country.CountryId.ToString()
            }).ToList();
        }
    }
}
