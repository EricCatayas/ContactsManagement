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
using ContactsManagement.Core.ServiceContracts.AzureBlobServices;
using Microsoft.AspNetCore.Identity;
using ContactsManagement.Core.Domain.IdentityEntities;
using ContactsManagement.Core.ServiceContracts.AccountManager;

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
        private readonly IImageUploaderService _imageUploaderService;
        private readonly IImageDeleterService _imageDeleterService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDemoUserService _demoUserService;

        public PersonsController(IPersonsGetterService personsGetterService, IPersonsAdderService personsAdderService, IPersonsDeleterService personsDeleterService, IPersonsSorterService personsSorterService, IPersonsUpdaterService personsUpdaterService, ICountriesService countriesService, IContactGroupsGetterService contactGroupsGetterService, IImageUploaderService imageUploaderService, IImageDeleterService imageDeleterService, UserManager<ApplicationUser> userManager, IDemoUserService demoUserService)
        {
            _personGetterService = personsGetterService;
            _personAdderService = personsAdderService;
            _personsSorterService = personsSorterService;
            _personsUpdaterService = personsUpdaterService;
            _personsDeleterService = personsDeleterService;
            _countriesService = countriesService;
            _contactGroupsGetterService = contactGroupsGetterService;
            _imageUploaderService = imageUploaderService;
            _imageDeleterService = imageDeleterService;
            _userManager = userManager;
            _demoUserService = demoUserService;
        }        
        [Route("[action]")]
        [AllowAnonymous]
        [TypeFilter(typeof(PersonsListActionFilter))]
        [ResponseHeaderActionFilter("Action-Key", "In-Index-Method", 1)]              
        [TokenResultFactoryFilter(true, "Auth-Key", "A-100")]
        public async Task<IActionResult> Index(string? searchProperty, string? searchString, int? groupId, string sortProperty = "Name", string sortOrder = "ASC", List<string>? error = null)
        {
             // ViewData handled in PersonsListActionFilter     
            ViewBag.Errors = error;
            string? userId = _userManager.GetUserId(User);
            Guid UserId;
            if (userId != null)
                UserId = Guid.Parse(userId);
            else
                UserId = _demoUserService.GetDemoUserId();

            List<PersonResponse>? personList = await _personGetterService.GetFilteredPersons(UserId, searchProperty, searchString);
            if(groupId != null)
            {
                personList = personList.Where(person => person.ContactGroups.Any(group => group.GroupId == groupId)).ToList();
            }
            List<ContactGroupResponse>? contactGroups = await _contactGroupsGetterService.GetAllContactGroups(UserId);
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
        public async Task<IActionResult> Create([FromForm] PersonAddRequest? personAddRequest,[FromForm] IFormFile? profileImage)
        {
            if (profileImage != null && profileImage.Length > 0 && profileImage.ContentType.StartsWith("image/"))
            {
                using (var memoryStream = new MemoryStream())
                {
                    await profileImage.CopyToAsync(memoryStream);
                    byte[] fileData = memoryStream.ToArray();
                    personAddRequest.ProfileBlobUrl = await _imageUploaderService.UploadImageAsync(fileData, profileImage.FileName);
                }
            }
            string? userId = _userManager.GetUserId(User);
            Guid UserId = Guid.Parse(userId);

            _ = await _personAdderService.AddPerson(personAddRequest, UserId);
            ViewBag.Success = "Person has been successfully added!";
            return View(new PersonAddRequest() { });
        }
        [HttpPost]
        [Route("[action]/{personId}")]
        [TypeFilter(typeof(PersonCreateAndEditActionFilter))]       
        [ServiceFilter(typeof(RedirectToIndexExceptionFilter))]
        public async Task<IActionResult> Edit([FromForm] PersonUpdateRequest personUpdateRequest,[FromForm] IFormFile? profileImage)
        {
            if (profileImage != null && profileImage.Length > 0 && profileImage.ContentType.StartsWith("image/"))
            {
                if(personUpdateRequest.ProfileBlobUrl != null)
                    _ = await _imageDeleterService.DeleteBlobFile(personUpdateRequest.ProfileBlobUrl);
                
                using (var memoryStream = new MemoryStream())
                {
                    await profileImage.CopyToAsync(memoryStream);
                    byte[] fileData = memoryStream.ToArray();
                    personUpdateRequest.ProfileBlobUrl = await _imageUploaderService.UploadImageAsync(fileData, profileImage.FileName);
                }
            }
                
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
            PersonResponse? person = await _personGetterService.GetPersonById(personId);
            if (person.ProfileBlobUrl != null)
                _ = await _imageDeleterService.DeleteBlobFile(person.ProfileBlobUrl);
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
            string? userId = _userManager.GetUserId(User);
            Guid UserId = Guid.Parse(userId);

            List<PersonResponse> allPersons = await _personGetterService.GetAllPersons(UserId);
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
            string? userId = _userManager.GetUserId(User);
            Guid UserId = Guid.Parse(userId);

            MemoryStream memoryStream = await _personGetterService.GetPersonsCSV(UserId);
            return File(memoryStream, "application/octet-stream", "Contacts.csv");
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> PersonsExcel()
        {
            string? userId = _userManager.GetUserId(User);
            Guid UserId = Guid.Parse(userId);

            MemoryStream memoryStream = await _personGetterService.GetPersonsEXCEL(UserId);
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Contacts.xlsx");
        }
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
