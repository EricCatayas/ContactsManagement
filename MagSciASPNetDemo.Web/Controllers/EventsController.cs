﻿using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.DTO.CompaniesManagement;
using ContactsManagement.Core.DTO.EventsManager;
using ContactsManagement.Core.ServiceContracts.EventsManager;
using ContactsManagement.Core.Services.EventsManager;
using ContactsManagement.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContactsManagement.Web.Controllers
{
    [Route("[controller]")]
    public class EventsController : Controller
    {
        private readonly IEventsAdderService _eventsAdderService;
        private readonly IEventsGetterService _eventsGetterService;
        private readonly IEventsUpdaterService _eventsUpdaterService;
        private readonly IEventsDeleterService _eventsDeleterService;
        List<string> eventColorOptions;
        List<string> eventTypeOptions;
        public EventsController(IEventsAdderService eventsAdderService, IEventsGetterService eventsGetterService, IEventsUpdaterService eventsUpdaterService, IEventsDeleterService eventsDeleterService) 
        {
            _eventsAdderService = eventsAdderService;
            _eventsGetterService = eventsGetterService;
            _eventsUpdaterService = eventsUpdaterService;
            _eventsDeleterService = eventsDeleterService;
            eventColorOptions = new List<string>()
            {
                "yellow","blue","green","grey","pink","brown","purple","orange"
            };
            eventTypeOptions = new List<string>()
            {
                "Task", "Reminder", "Activity", "Meeting", "Conference", "Webinar", "Workshop", "Birthday", "Call", "Email", "Occassion", "Appointment"
            };
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Index(List<string>? error = null, bool isActiveEvent = true)
        {
            List<EventResponse>? events = await _eventsGetterService.GetEvents();

            if (isActiveEvent)
            {
                events = events.Where(temp => temp.isActive).ToList();
                ViewBag.IsActiveEvents = true;
            }
            else
            {
                events = events.Where(temp => !temp.isActive).ToList();
                ViewBag.IsActiveEvents = false;
            }
            return View(events);
        }
        [Route("[action]")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.ThemeColors = eventColorOptions;
            ViewBag.TypeOptions = eventTypeOptions;
            return View();
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Create(EventAddRequest eventAddRequest) //TODO StartDate and EndDate Kuan
        {
            ViewBag.ThemeColors = eventColorOptions;
            ViewBag.TypeOptions = eventTypeOptions;
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
        /*[Route("[action]")]            // How bout don't make a mistake? :^)
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
        }*/
        /*[Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Edit(EventUpdateRequest eventUpdateRequest)
        {
            bool isUpdated = await _eventsUpdaterService.UpdateEvent(eventUpdateRequest);
            return isUpdated ? StatusCode(200) : StatusCode(500);
        }*/
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