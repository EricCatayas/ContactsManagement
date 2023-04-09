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
        public async Task<Event?> GetEvent(int EventId, Guid userId)
        {
            Event? @event = await _db.Events.FirstOrDefaultAsync(e => e.EventId == EventId && e.UserId == userId);
            return @event;
        }

        public async Task<List<Event>?> GetEvents(Guid userId)
        {
            return await _db.Events.Where(temp => temp.UserId == userId).ToListAsync();
        }
    }
}
