using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.EventsManager;
using ContactsManagement.Core.DTO.EventsManager;
using ContactsManagement.Core.Exceptions;
using ContactsManagement.Core.ServiceContracts.AccountManager;
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
        private readonly ISignedInUserService _signedInUserService;

        public EventsUpdaterService(IEventsUpdaterRepository eventsUpdaterRepository, IEventsGetterRepository eventsGetterRepository, ISignedInUserService signedInUserService)
        {
            _eventsUpdaterRepository = eventsUpdaterRepository;
            _eventsGetterRepository = eventsGetterRepository;
            _signedInUserService = signedInUserService;
        }
        public async Task<bool> UpdateEvent(EventUpdateRequest eventUpdateRequest)
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();

            eventUpdateRequest.UserId = userId;
            if (eventUpdateRequest.isCompleted)
            {
                eventUpdateRequest.Status = "Completed";
            }
            return await _eventsUpdaterRepository.UpdateEvent(eventUpdateRequest.ToEvent());
        }

        public async Task<bool> UpdateEventCompletion(int eventId)
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();

            Event? @event = await _eventsGetterRepository.GetEvent(eventId, (Guid)userId);
            if (@event == null)
                return false;
            @event.Status = "Completed";
            @event.isActive = false;
            return await _eventsUpdaterRepository.UpdateEvent(@event);
        }
    }
}
