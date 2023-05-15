using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.EventsManager;
using ContactsManagement.Core.DTO.EventsManager;
using ContactsManagement.Core.Enums.ContactsManager;
using ContactsManagement.Core.Exceptions;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.EventsManager;
using ContactsManagement.Core.Services.AccountManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.EventsManager
{
    public class EventsGetterService : IEventsGetterService
    {
        private readonly IEventsGetterRepository _eventsGetterRepository;
        private readonly IEventsStatusUpdaterRepository _eventsStatusUpdaterRepository;
        private readonly ISignedInUserService _signedInUserService;

        public EventsGetterService(IEventsGetterRepository eventsGetterRepository, IEventsStatusUpdaterRepository eventsStatusUpdaterRepository, ISignedInUserService signedInUserService)
        {
            _eventsGetterRepository = eventsGetterRepository;
            _eventsStatusUpdaterRepository = eventsStatusUpdaterRepository;
            _signedInUserService = signedInUserService;
        }
        public async Task<EventResponse?> GetEventById(int eventId)
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();
            Event? @event = await _eventsGetterRepository.GetEvent(eventId, (Guid)userId);
            if (@event == null) { return null; }
            //Updation of Status

            return @event.ToEventResponse();
        }

        public async Task<List<EventResponse>?> GetEvents()
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();

            List<Event>? events = await _eventsGetterRepository.GetEvents((Guid)userId);
            if (events == null) return null;

            //Updation of Status
            events = await _eventsStatusUpdaterRepository.UpdateEventsStatus(events);
            return events.Select(temp => temp.ToEventResponse()).ToList(); 
        }

        public async Task<List<EventResponse>?> GetFilteredEvents(StatusType statusType)
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();

            List<Event>? events = new List<Event>();
            if (statusType == StatusType.Completed)
            {
                events = await _eventsGetterRepository.GetFilteredEvents(temp => !temp.isActive, (Guid)userId);
            }
            else
            {
                events = await _eventsGetterRepository.GetFilteredEvents(temp => temp.isActive, (Guid)userId);
            }
            events = await _eventsStatusUpdaterRepository.UpdateEventsStatus(events);
            return events.Select(temp => temp.ToEventResponse()).ToList();
        }
    }
}
