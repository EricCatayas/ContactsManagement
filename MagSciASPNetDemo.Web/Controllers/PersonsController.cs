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
using ContactsManagement.Core.ServiceContracts.CompaniesManagement;
using ContactsManagement.Core.Services.CompaniesManagement;
using ContactsManagement.Core.ServiceContracts.Others;

namespace ContactsManagement.Web.Controllers
{
    [Route("persons")]
    [TypeFilter(typeof(AlwaysRunResultFilter))]
    [ResponseHeaderActionFilter("Controller-Key", "In-PersonsController",3)]
    [TypeFilter(typeof(RedirectToIndexExceptionFilter))]
    public class PersonsController : Controller
    {
        private readonly IPersonsGetterService _personGetterService;
        private readonly IPersonsAdderService _personAdderService;
        private readonly IPersonsDeleterService _personsDeleterService;
        private readonly IPersonsGroupIdFilteredGetterService _personsGroupIdFilteredGetterService;
        private readonly IPersonsSorterService _personsSorterService;
        private readonly IPersonsUpdaterService _personsUpdaterService;
        private readonly ICountriesService _countriesService;
        private readonly ICompanyAdderByNameService _companyAdderByNameService;
        private readonly IImageUploaderService _imageUploaderService;
        private readonly IImageDeleterService _imageDeleterService;

        public PersonsController(IPersonsGetterService personsGetterService, IPersonsAdderService personsAdderService, IPersonsDeleterService personsDeleterService, IPersonsSorterService personsSorterService, IPersonsUpdaterService personsUpdaterService, IPersonsGroupIdFilteredGetterService personsGroupIdFilteredGetterService, ICountriesService countriesService, IImageUploaderService imageUploaderService, IImageDeleterService imageDeleterService, ICompanyAdderByNameService companyAdderByNameService)
        {
            _personGetterService = personsGetterService;
            _personAdderService = personsAdderService;
            _personsSorterService = personsSorterService;
            _personsUpdaterService = personsUpdaterService;
            _personsDeleterService = personsDeleterService;
            _personsGroupIdFilteredGetterService = personsGroupIdFilteredGetterService;
            _countriesService = countriesService;
            _companyAdderByNameService = companyAdderByNameService;
            _imageUploaderService = imageUploaderService;
            _imageDeleterService = imageDeleterService;
        }        
        [Route("[action]")]
        [AllowAnonymous]
        [TypeFilter(typeof(PersonsListActionFilter))]
        [ResponseHeaderActionFilter("Action-Key", "In-Index-Method", 1)]        
        public async Task<IActionResult> Index(string? searchProperty, string? searchString, int? groupId, string? displayType, string sortProperty = "Name", string sortOrder = "ASC", List<string>? errors = null)
        {
            ViewBag.Errors = errors;
            List<PersonResponse>? personList = new List<PersonResponse>() { };
            if(groupId != null)
            {
                personList = await _personsGroupIdFilteredGetterService.GetFilteredPersons(groupId, searchProperty, searchString);
            } else
            {
                personList = await _personGetterService.GetFilteredPersons(searchProperty, searchString);
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
                    try
                    {
                        personAddRequest.ProfileBlobUrl = await _imageUploaderService.UploadImageAsync(fileData, profileImage.FileName);
                    }
                    catch
                    {
                        ViewBag.Error = new List<string>() { "An error occured while uploading the image." };
                    }
                }
            }

            if (personAddRequest.CompanyName != null)
                personAddRequest.CompanyId = await _companyAdderByNameService.AddCompanyByName(personAddRequest.CompanyName);
            try
            {
                _ = await _personAdderService.AddPerson(personAddRequest);
                ViewBag.Success = "Person has been successfully added!";
            }
            catch
            {
                //Delete Image if exception occurs during transaction
                if(personAddRequest.ProfileBlobUrl != null)
                    await _imageDeleterService.DeleteBlobFile(personAddRequest.ProfileBlobUrl);
            }
            return View(new PersonAddRequest() { });
        }
        [HttpPost]
        [Route("[action]/{personId}")]
        [TypeFilter(typeof(PersonCreateAndEditActionFilter))]     
        public async Task<IActionResult> Edit([FromForm] PersonUpdateRequest personUpdateRequest,[FromForm] IFormFile? profileImage)
        {
            if (profileImage != null && profileImage.Length > 0 && profileImage.ContentType.StartsWith("image/"))
            {
                bool isImageDeleted = false;
                if (personUpdateRequest.ProfileBlobUrl != null)
                    isImageDeleted = await _imageDeleterService.DeleteBlobFile(personUpdateRequest.ProfileBlobUrl);

                if (isImageDeleted)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await profileImage.CopyToAsync(memoryStream);
                        byte[] fileData = memoryStream.ToArray();
                        try
                        {
                            personUpdateRequest.ProfileBlobUrl = await _imageUploaderService.UploadImageAsync(fileData, profileImage.FileName);
                        }
                        catch
                        {
                            ViewBag.Error = new List<string>() { "An error occured while uploading the image." };
                        }
                    }
                }
                else
                {
                    ViewBag.Error = new List<string>() { "An error occured while updating the image." };
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
        public async Task<IActionResult> Edit(Guid? personId, List<string>? errors = null)
        {
            PersonResponse? matchingPerson = await _personGetterService.GetPersonById(personId);
            if (matchingPerson == null)
                return RedirectToAction("Index");

            ViewBag.Errors = errors;
            ViewBag.Countries = await LoadCountrySelectItems();
            PersonUpdateRequest personToUpdate = matchingPerson.ToPersonUpdateRequest();
            return View(personToUpdate);
        }
        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> Delete(Guid personId)
        {
            bool isPersonDeleted = false;
            
            PersonResponse? person = await _personGetterService.GetPersonById(personId);
            if (person.ProfileBlobUrl != null)
            {
                bool isImageDeleted = false;
                isImageDeleted = await _imageDeleterService.DeleteBlobFile(person.ProfileBlobUrl);

                if (isImageDeleted)
                    isPersonDeleted = await _personsDeleterService.DeletePerson(personId);
            }
            else
            {
                isPersonDeleted = await _personsDeleterService.DeletePerson(personId);
            }
            
            
            if (isPersonDeleted)
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
                return RedirectToAction("Index", new { errors = "Table is empty" });

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
            return File(memoryStream, "application/octet-stream", "Contacts.csv");
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> PersonsExcel()
        {
            MemoryStream memoryStream = await _personGetterService.GetPersonsEXCEL();
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
