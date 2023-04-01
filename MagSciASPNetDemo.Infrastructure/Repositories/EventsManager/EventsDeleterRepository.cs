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
    public class EventsDeleterRepository : IEventsDeleterRepository
    {
        private readonly ApplicationDbContext _db;

        public EventsDeleterRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> DeleteEvent(int eventId)
        {
            Event? @event = await _db.Events.FirstOrDefaultAsync(e => e.EventId == eventId);
            if (@event == null) return false;

            _db.Events.Remove(@event);
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
