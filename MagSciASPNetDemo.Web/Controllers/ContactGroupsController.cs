using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.IdentityEntities;
using ContactsManagement.Core.DTO.CompaniesManagement;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactGroupsServices;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactTagsServices;
using ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices;
using ContactsManagement.Core.Services.ContactsManager.ContactGroups;
using ContactsManagement.Core.Services.ContactsManager.ContactTags;
using ContactsManagement.Web.Filters.ExceptionFilters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManagement.Web.Controllers
{
    [Route("[controller]")]
    [TypeFilter(typeof(RedirectToIndexExceptionFilter))]
    public class ContactGroupsController : Controller
    {
        private readonly IContactGroupsAdderService _contactGroupsAdderService;
        private readonly IPersonsGetterService _personsGetterService;
        private readonly IContactTagsGetterService _contactTagsGetterService;
        private readonly IContactGroupsGetterService _contactGroupsGetterService;
        private readonly IContactGroupsDeleterService _contactGroupsDeleterService;

        public ContactGroupsController(IContactGroupsAdderService contactGroupsAdderService, IContactGroupsGetterService contactGroupsGetterService, IContactGroupsDeleterService contactGroupsDeleterService, IPersonsGetterService personsGetterService, IContactTagsGetterService contactTagsGetterService)
        {
            _contactGroupsAdderService = contactGroupsAdderService;
            _contactGroupsGetterService = contactGroupsGetterService;
            _contactGroupsDeleterService = contactGroupsDeleterService;
            _personsGetterService = personsGetterService;
            _contactTagsGetterService = contactTagsGetterService;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Index(List<string>? errors)
        {
            ViewBag.Persons = await _personsGetterService.GetAllPersons();
            ViewBag.ContactGroups = await _contactGroupsGetterService.GetAllContactGroups();
            ViewBag.ContactTags = await _contactTagsGetterService.GetAllContactTags();
            ViewBag.Errors = errors;
            return View();
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Create(ContactGroupAddRequest contactGroupAddRequest)
        {
            if (ModelState.IsValid)
            {
                ContactGroupResponse contactGroup = await _contactGroupsAdderService.AddContactGroup(contactGroupAddRequest);
                return new RedirectToActionResult("Index", "Persons", new { groupId = contactGroup.GroupId });
            }
            else
            {
                ViewBag.Errors = ModelState.Values.SelectMany(V => V.Errors).Select(err => err.ErrorMessage).ToList();
                ViewBag.Persons = await _personsGetterService.GetAllPersons();
                ViewBag.ContactGroups = await _contactGroupsGetterService.GetAllContactGroups();
                ViewBag.ContactTags = await _contactTagsGetterService.GetAllContactTags();
                return View("Index");
            }
            
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Delete(int contactGroupId)
        {
            bool isDeleted = await _contactGroupsDeleterService.DeleteContactGroup(contactGroupId);
            if (isDeleted)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }
}
