using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.EventsManager;
using ContactsManagement.Core.DTO.EventsManager;
using ContactsManagement.Core.Enums.ContactsManager;
using ContactsManagement.Core.ServiceContracts.EventsManager;
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

        public EventsGetterService(IEventsGetterRepository eventsGetterRepository, IEventsStatusUpdaterRepository eventsStatusUpdaterRepository)
        {
            _eventsGetterRepository = eventsGetterRepository;
            _eventsStatusUpdaterRepository = eventsStatusUpdaterRepository;
        }
        public async Task<EventResponse?> GetEventById(int eventId, Guid userId)
        {
            Event? @event = await _eventsGetterRepository.GetEvent(eventId, userId);
            if (@event == null) { return null; }
            //Updation of Status

            return @event.ToEventResponse();
        }

        public async Task<List<EventResponse>?> GetEvents(Guid userId)
        {
            List<Event>? events = await _eventsGetterRepository.GetEvents(userId);
            if (events == null) return null;

            events = events.OrderBy(temp => temp.StartDate).ToList();
            //Updation of Status
            events = await _eventsStatusUpdaterRepository.UpdateEventsStatus(events);
            return events.Select(temp => temp.ToEventResponse()).ToList(); 
        }

        public List<EventResponse>? GetFilteredEvents(List<EventResponse>? events, StatusType statusType)
        {
            // Simplification for now
            throw new NotImplementedException();
        }

    }
}
