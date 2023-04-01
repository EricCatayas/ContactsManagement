using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.EventsManager;
using ContactsManagement.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Infrastructure.Repositories.EventsManager
{
    public class EventsAdderRepository : IEventsAdderRepository
    {
        private readonly ApplicationDbContext _db;

        public EventsAdderRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Event> AddEvent(Event @event)
        {
            _db.Events.Add(@event);
            await _db.SaveChangesAsync();
            return @event;
        }
    }
}
