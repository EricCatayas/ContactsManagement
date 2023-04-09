using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.EventsManager;
using ContactsManagement.Core.ServiceContracts.EventsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.EventsManager
{
    public class EventsSeederService : IEventsSeederService
    {
        private readonly IEventsAdderRepository _eventsAdderRepository;

        public EventsSeederService(IEventsAdderRepository eventsAdderRepository)
        {
            _eventsAdderRepository = eventsAdderRepository;
        }

        public async Task SeedUserEvents(Guid userId)
        {
            Event @event = new Event()
            {
                Title = " Create to-do lists or reminders",
                Description = " Keep track of your schedule to ensure you don't miss important events and tasks.",
                StartDate = DateTime.Now,
                UserId = userId,
            };
            await _eventsAdderRepository.AddEvent(@event);
        }
    }
}
