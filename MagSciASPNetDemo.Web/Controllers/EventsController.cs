using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.IdentityEntities;
using ContactsManagement.Core.DTO.CompaniesManagement;
using ContactsManagement.Core.DTO.EventsManager;
using ContactsManagement.Core.Enums.ContactsManager;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.EventsManager;
using ContactsManagement.Core.Services.EventsManager;
using ContactsManagement.Web.Filters.ExceptionFilters;
using ContactsManagement.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContactsManagement.Web.Controllers
{
    [Route("[controller]")]
    [TypeFilter(typeof(RedirectToIndexExceptionFilter))]
    public class EventsController : Controller
    {
        private readonly IEventsAdderService _eventsAdderService;
        private readonly IEventsGetterService _eventsGetterService;
        private readonly IEventsUpdaterService _eventsUpdaterService;
        private readonly IEventsDeleterService _eventsDeleterService;
        private readonly IDemoUserService _demoUserService;
        public EventsController(IEventsAdderService eventsAdderService, IEventsGetterService eventsGetterService, IEventsUpdaterService eventsUpdaterService, IEventsDeleterService eventsDeleterService) 
        {
            _eventsAdderService = eventsAdderService;
            _eventsGetterService = eventsGetterService;
            _eventsUpdaterService = eventsUpdaterService;
            _eventsDeleterService = eventsDeleterService;
        }
        [Route("[action]")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(List<string>? errors = null, bool isActiveEvent = true)
        {

            List<EventResponse>? events = new List<EventResponse>();
            if (isActiveEvent)
            {
                events = await _eventsGetterService.GetFilteredEvents(statusType: StatusType.Active);
                ViewBag.IsActiveEvents = true;
            }
            else
            {
                events = await _eventsGetterService.GetFilteredEvents(statusType: StatusType.Completed);
                ViewBag.IsActiveEvents = false;
            }
            return View(events);
        }
        [Route("[action]")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Create(EventAddRequest eventAddRequest) 
        {
            if (ModelState.IsValid)
            {
                _ = await _eventsAdderService.AddEvent(eventAddRequest);
                ViewBag.Success = "Event has been added";
                return View();
            }
            else
            {
                ViewBag.Errors = ModelState.Values.SelectMany(V => V.Errors).Select(err => err.ErrorMessage).ToList();
                return View(eventAddRequest);
            }
        }
        [Route("[action]")]            // How bout don't make a mistake? :^)
        [HttpGet]
        public async Task<IActionResult> Edit(int eventId)
        {
            EventResponse? @event = await _eventsGetterService.GetEventById(eventId);
            if (@event == null)
            {
                return StatusCode(404);
            }
            EventUpdateRequest eventUpdateRequest = @event.ToEventUpdateRequest();
            return View(eventUpdateRequest);
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Edit(EventUpdateRequest eventUpdateRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(V => V.Errors).Select(err => err.ErrorMessage).ToList();
                return View(eventUpdateRequest);
            }
            bool isUpdated = await _eventsUpdaterService.UpdateEvent(eventUpdateRequest);
            if (isUpdated)
            {
                return RedirectToAction("Index");
            } else {
                ViewBag.Errors = new List<string>() { "Error: Something went wrong. Please try again later"};
                return View(eventUpdateRequest);
            }
        }
        [Route("[action]")]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int eventId)
        {
            bool isDeleted = await _eventsDeleterService.DeleteEvent(eventId);
            return isDeleted ? StatusCode(200) : StatusCode(500);
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> EventCompleted([FromQuery] int eventId)
        {
            bool isUpdated = await _eventsUpdaterService.UpdateEventCompletion(eventId);
            return isUpdated ? StatusCode(200) : StatusCode(500);
        }
    }
}
