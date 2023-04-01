using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.EventsManager;
using ContactsManagement.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Infrastructure.Repositories.EventsManager
{
    public class EventsGetterRepository : IEventsGetterRepository
    {
        private readonly ApplicationDbContext _db;

        public EventsGetterRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Event?> GetEvent(int EventId)
        {
            Event? @event = await _db.Events.FirstOrDefaultAsync(e => e.EventId == EventId);
            return @event;
        }

        public async Task<List<Event>?> GetEvents()
        {
            return await _db.Events.ToListAsync();
        }
    }
}
