using ContactsManagement.Core.Domain.RepositoryContracts.EventsManager;
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

        public EventsDeleterService(IEventsDeleterRepository eventsDeleterRepository)
        {
            _eventsDeleterRepository = eventsDeleterRepository;
        }
        public async Task<bool> DeleteEvent(int eventId)
        {
            return await _eventsDeleterRepository.DeleteEvent(eventId);
        }
    }
}
