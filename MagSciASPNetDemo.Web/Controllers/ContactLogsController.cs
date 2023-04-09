using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.IdentityEntities;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.Enums.ContactsManager;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactLogsServices;
using ContactsManagement.Core.Services.ContactsManager.ContactLogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManagement.Web.Controllers
{
    [Route("[controller]")]
    public class ContactLogsController : Controller
    {
        private readonly IContactLogsAdderService _contactLogsAdderService;
        private readonly IContactLogsGetterService _contactLogsGetterService;
        private readonly IContactLogsSorterService _contactLogsSorterService;
        private readonly IContactLogsUpdaterService _contactLogsUpdaterService;
        private readonly IContactLogsDeleterService _contactLogsDeleterService;
        private readonly IDemoUserService _guestUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Dictionary<string, string> sortProperties;
        private readonly List<string> communicationTypes;

        public ContactLogsController(IContactLogsAdderService contactLogsAdderService, IContactLogsGetterService contactLogsGetterService, IContactLogsSorterService contactLogsSorterService, IContactLogsUpdaterService contactLogsUpdaterService, IContactLogsDeleterService contactLogsDeleterService, UserManager<ApplicationUser> userManager, IDemoUserService guestUserService)
        {
            _contactLogsAdderService = contactLogsAdderService;
            _contactLogsGetterService = contactLogsGetterService;
            _contactLogsSorterService = contactLogsSorterService;
            _contactLogsUpdaterService = contactLogsUpdaterService;
            _contactLogsDeleterService = contactLogsDeleterService;
            _guestUserService = guestUserService;
            _userManager = userManager;
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
        public async Task<IActionResult> Index(string? searchText, string sortProperty = "PersonLog", string sortOrder = "ASC", List<string>? error = null)
        {
            string? userId = _userManager.GetUserId(User);
            Guid UserId;
            if (userId != null)
                UserId = Guid.Parse(userId);
            else
                UserId = _guestUserService.GetDemoUserId();

            List<ContactLogResponse>? contactLogs = await _contactLogsGetterService.GetContactLogs(UserId);
            if (contactLogs != null)
            {
                contactLogs = _contactLogsGetterService.GetFilteredContactLogs(contactLogs, searchText);
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
            ViewBag.Errors = error;
            return View(contactLogs);
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Create(ContactLogAddRequest contactLogAddRequest)
        {
            if (ModelState.IsValid)
            {
                string? userId = _userManager.GetUserId(User);
                Guid UserId = Guid.Parse(userId);

                await _contactLogsAdderService.AddContactLog(contactLogAddRequest, UserId);
                return RedirectToAction("Edit", "Persons", new { personId = contactLogAddRequest.PersonId });
            }
            else
            {
                List<string> errors = ModelState.Values.SelectMany(V => V.Errors).Select(err => err.ErrorMessage).ToList();
                return RedirectToAction("Edit", "Persons", new { personId = contactLogAddRequest.PersonId, error = errors });
            }
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Details(int contactLogId)
        {
            string? userId = _userManager.GetUserId(User);
            Guid UserId = Guid.Parse(userId);

            ContactLogResponse? contactLog = await _contactLogsGetterService.GetContactLogById(contactLogId, UserId);
            if (contactLog == null)
                return StatusCode(500);
            ContactLogUpdateRequest contactLogUpdate = contactLog.ToContactLogUpdateRequest();
            ViewBag.CommunicationTypes = communicationTypes;
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
            string? userId = _userManager.GetUserId(User);
            Guid UserId = Guid.Parse(userId);

            bool isDeleted = await _contactLogsDeleterService.DeleteContactLog(contactLogId, UserId);

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
