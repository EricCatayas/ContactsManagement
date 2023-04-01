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
    public class EventsUpdaterRepository : IEventsUpdaterRepository
    {
        private readonly ApplicationDbContext _db;

        public EventsUpdaterRepository(ApplicationDbContext db) 
        {
            _db = db;
        }
        public async Task<bool> UpdateEvent(Event @event)
        {
            Event? matchingEvent = await _db.Events.FirstOrDefaultAsync(temp => temp.EventId == @event.EventId);
            if (matchingEvent == null)
                return false;
            matchingEvent.Title = @event.Title;
            matchingEvent.Type = @event.Type;
            matchingEvent.Description = @event.Description;
            matchingEvent.ThemeColor = @event.ThemeColor;
            matchingEvent.StartDate = @event.StartDate;
            matchingEvent.EndDate = @event.EndDate;
            matchingEvent.isActive = @event.isActive;
            int savedChanges = await _db.SaveChangesAsync();
            return savedChanges > 0;
        }
    }
}
