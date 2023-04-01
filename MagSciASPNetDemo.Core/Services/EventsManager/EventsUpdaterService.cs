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
    public class EventsUpdaterService : IEventsUpdaterService
    {
        private readonly IEventsUpdaterRepository _eventsUpdaterRepository;
        private readonly IEventsGetterRepository _eventsGetterRepository;

        public EventsUpdaterService(IEventsUpdaterRepository eventsUpdaterRepository, IEventsGetterRepository eventsGetterRepository)
        {
            _eventsUpdaterRepository = eventsUpdaterRepository;
            _eventsGetterRepository = eventsGetterRepository;
        }
        public async Task<bool> UpdateEvent(EventUpdateRequest eventUpdateRequest)
        {
            if (eventUpdateRequest.StartDate > eventUpdateRequest.EndDate)
                return false;
            if(eventUpdateRequest.isCompleted)
            {
                eventUpdateRequest.Status = "Completed";
            }
            return await _eventsUpdaterRepository.UpdateEvent(eventUpdateRequest.ToEvent());
        }

        public async Task<bool> UpdateEventCompletion(int eventId)
        {
            Event? @event = await _eventsGetterRepository.GetEvent(eventId);
            if (@event == null)
                return false;
            @event.Status = "Completed";
            @event.isActive = false;
            return await _eventsUpdaterRepository.UpdateEvent(@event);
        }
    }
}
