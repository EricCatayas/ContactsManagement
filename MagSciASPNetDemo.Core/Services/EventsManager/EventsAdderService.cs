using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.EventsManager;
using ContactsManagement.Core.DTO.EventsManager;
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
    public class EventsAdderService : IEventsAdderService
    {
        private readonly IEventsAdderRepository _eventsAdderRepository;
        private readonly IEventsStatusUpdaterRepository _eventsStatusUpdaterRepository;
        private readonly ISignedInUserService _signedInUserService;

        public EventsAdderService(IEventsAdderRepository eventsAdderRepository, IEventsStatusUpdaterRepository eventsStatusUpdaterRepository, ISignedInUserService signedInUserService)
        {
            _eventsAdderRepository = eventsAdderRepository;
            _eventsStatusUpdaterRepository = eventsStatusUpdaterRepository;
            _signedInUserService = signedInUserService;
        }
        public async Task<EventResponse> AddEvent(EventAddRequest? eventAddRequest)
        {
            if (eventAddRequest == null)
                throw new ArgumentNullException();
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();

            Event @event = eventAddRequest.ToEvent();
            @event.UserId = userId;
            @event.Status = _eventsStatusUpdaterRepository.GetUpdatedStatus(@event.StartDate, @event.EndDate);
            @event.LastUpdatedDate = DateTime.Now;
            @event = await _eventsAdderRepository.AddEvent(@event);
            return @event.ToEventResponse();
        }

    }
}
