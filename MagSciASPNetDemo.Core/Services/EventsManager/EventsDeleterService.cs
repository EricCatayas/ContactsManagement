using ContactsManagement.Core.Domain.RepositoryContracts.EventsManager;
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
    public class EventsDeleterService : IEventsDeleterService
    {
        private readonly IEventsDeleterRepository _eventsDeleterRepository;
        private readonly ISignedInUserService _signedInUserService;

        public EventsDeleterService(IEventsDeleterRepository eventsDeleterRepository, ISignedInUserService signedInUserService)
        {
            _eventsDeleterRepository = eventsDeleterRepository;
            _signedInUserService = signedInUserService;
        }
        public async Task<bool> DeleteEvent(int eventId)
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();
            return await _eventsDeleterRepository.DeleteEvent(eventId, (Guid)userId);
        }
    }
}
