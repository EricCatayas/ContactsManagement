using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.IdentityEntities;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.Enums.ContactsManager;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactLogsServices;
using ContactsManagement.Core.Services.ContactsManager.ContactLogs;
using ContactsManagement.Web.Filters.ExceptionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManagement.Web.Controllers
{
    [Route("[controller]")]
    [TypeFilter(typeof(RedirectToIndexExceptionFilter))]
    public class ContactLogsController : Controller
    {
        private readonly IContactLogsAdderService _contactLogsAdderService;
        private readonly IContactLogsGetterService _contactLogsGetterService;
        private readonly IContactLogsSorterService _contactLogsSorterService;
        private readonly IContactLogsUpdaterService _contactLogsUpdaterService;
        private readonly IContactLogsDeleterService _contactLogsDeleterService;
        private readonly Dictionary<string, string> sortProperties;
        private readonly List<string> communicationTypes;

        public ContactLogsController(IContactLogsAdderService contactLogsAdderService, IContactLogsGetterService contactLogsGetterService, IContactLogsSorterService contactLogsSorterService, IContactLogsUpdaterService contactLogsUpdaterService, IContactLogsDeleterService contactLogsDeleterService)
        {
            _contactLogsAdderService = contactLogsAdderService;
            _contactLogsGetterService = contactLogsGetterService;
            _contactLogsSorterService = contactLogsSorterService;
            _contactLogsUpdaterService = contactLogsUpdaterService;
            _contactLogsDeleterService = contactLogsDeleterService;
            sortProperties = new Dictionary<string, string>()
            {
                { "Person Log", nameof(ContactLogResponse.PersonLog) },
                { "Date Created", nameof(ContactLogResponse.DateCreated) },
                { "Log Title", nameof(ContactLogResponse.LogTitle) },
            };
            communicationTypes = new List<string>()
            {
                "Phone", "In-Person", "Email", "Instant Messaging", "Video Conferencing", "Social Media", "Postal Mail", "Fax",
            };
        }
        [Route("[action]")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string? searchText, string sortProperty = "PersonLog", string sortOrder = "ASC", List<string>? errors = null)
        {
            List<ContactLogResponse>? contactLogs = new List<ContactLogResponse>();
            if(searchText != null)
            {
                contactLogs = await _contactLogsGetterService.GetFilteredContactLogs(searchText);
            }
            else
            {
                contactLogs = await _contactLogsGetterService.GetContactLogs();
            }
            if (contactLogs != null)
            {
                switch (sortOrder)
                {
                    case "ASC":
                        contactLogs = _contactLogsSorterService.GetSortedContactLogs(contactLogs, sortProperty, SortOrderOptions.ASC);
                        break;
                    case "DESC":
                        contactLogs = _contactLogsSorterService.GetSortedContactLogs(contactLogs, sortProperty, SortOrderOptions.DESC);
                        break;
                }
            }
            ViewBag.searchString = searchText;
            ViewBag.sortProperty = sortProperty;
            ViewBag.sortOrder = sortOrder;
            ViewBag.sortProperties = sortProperties;
            ViewBag.Errors = errors;
            return View(contactLogs);
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Create(ContactLogAddRequest contactLogAddRequest)
        {
            if (ModelState.IsValid)
            {

                await _contactLogsAdderService.AddContactLog(contactLogAddRequest);
                return RedirectToAction("Edit", "Persons", new { personId = contactLogAddRequest.PersonId });
            }
            else
            {
                List<string> Errors = ModelState.Values.SelectMany(V => V.Errors).Select(err => err.ErrorMessage).ToList();
                return RedirectToAction("Edit", "Persons", new { personId = contactLogAddRequest.PersonId, errors = Errors });
            }
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Details(int contactLogId)
        {
            ViewBag.CommunicationTypes = communicationTypes;
            ContactLogResponse? contactLog = await _contactLogsGetterService.GetContactLogById(contactLogId);
            if (contactLog == null)
                return StatusCode(500);
            ContactLogUpdateRequest contactLogUpdate = contactLog.ToContactLogUpdateRequest();
            return View(contactLogUpdate);
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Edit(ContactLogUpdateRequest contactLogUpdateRequest)
        {
            ViewBag.CommunicationTypes = communicationTypes;
            if (ModelState.IsValid)
            {
                await _contactLogsUpdaterService.UpdateContactLog(contactLogUpdateRequest);
                return View("Details", contactLogUpdateRequest);
            }
            else
            {
                List<string> errors = ModelState.Values.SelectMany(V => V.Errors).Select(err => err.ErrorMessage).ToList();
                ViewBag.Errors = errors;
                return View("Details", contactLogUpdateRequest);
            }
        }
        [Route("[action]")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int contactLogId)
        {
            bool isDeleted = await _contactLogsDeleterService.DeleteContactLog(contactLogId);

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
