using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.EventsManager;
using ContactsManagement.Core.DTO.EventsManager;
using ContactsManagement.Core.ServiceContracts.EventsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.EventsManager
{
    public class EventsAdderService : IEventsAdderService
    {
        private readonly IEventsAdderRepository _eventsAdderRepository;
        private readonly IEventsStatusUpdaterRepository _eventsStatusUpdaterRepository;

        public EventsAdderService(IEventsAdderRepository eventsAdderRepository, IEventsStatusUpdaterRepository eventsStatusUpdaterRepository)
        {
            _eventsAdderRepository = eventsAdderRepository;
            _eventsStatusUpdaterRepository = eventsStatusUpdaterRepository;
        }
        public async Task<EventResponse> AddEvent(EventAddRequest eventAddRequest)
        {
            Event @event = eventAddRequest.ToEvent();
            @event.Status = _eventsStatusUpdaterRepository.GetUpdatedStatus(@event.StartDate, @event.EndDate);
            @event.LastUpdatedDate = DateTime.Now;
            @event = await _eventsAdderRepository.AddEvent(@event);
            return @event.ToEventResponse();
        }

    }
}
