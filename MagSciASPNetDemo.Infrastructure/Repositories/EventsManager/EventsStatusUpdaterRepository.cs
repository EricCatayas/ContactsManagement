using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.EventsManager;
using ContactsManagement.Core.ServiceContracts.EventsManager;
using ContactsManagement.Infrastructure.DbContexts;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Infrastructure.Repositories.EventsManager
{
    public class EventsStatusUpdaterRepository : IEventsStatusUpdaterRepository
    {
        private readonly ApplicationDbContext _db;

        public EventsStatusUpdaterRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Event?> UpdateEventStatus(Event? @event)
        {
            if (@event == null)
                return null;
            if (@event.isActive)
            {
                if ((DateTime.Now - @event.LastUpdatedDate).TotalHours > 12)
                {
                    @event.Status = this.GetUpdatedStatus(@event.StartDate, @event.EndDate);
                    @event.LastUpdatedDate = DateTime.Now;
                }
                int changes = await _db.SaveChangesAsync();
            }
            return @event;
        }
        public async Task<List<Event>?> UpdateEventsStatus(List<Event>? events)
        {
            if (events.IsNullOrEmpty())
                return events;

            DateTime currentDate = DateTime.Now;
            Event @event = events[0];

            if ((currentDate - @event.LastUpdatedDate).TotalHours > 12)
            {
                events.ForEach(temp =>
                {
                    if(temp.isActive)
                    {                    
                        temp.Status = this.GetUpdatedStatus(temp.StartDate, temp.EndDate);
                        temp.LastUpdatedDate = currentDate;
                    }                
                });
            int changes =  await _db.SaveChangesAsync();
            }
            return events;
        }
        public string GetUpdatedStatus(DateTime? StartDate, DateTime? EndDate)
        {
            DateTime currentDate = DateTime.Now.Date;
            if (StartDate != null && EndDate != null)
            {
                if (StartDate <= currentDate && EndDate <= currentDate)
                {
                    return "Pending Completion";
                }
                else if (StartDate <= currentDate && EndDate >= currentDate)
                {
                    return "In Progress";
                }
                else if (StartDate.Value.Date == currentDate.AddDays(1).Date)
                {
                    return "Tomorrow";
                }
                else
                {
                    int daysRemaining = (StartDate.Value.Date - currentDate.Date).Days;
                    return $"Due in {daysRemaining} day/s";
                }
            }
            else if (StartDate != null)
            {
                if (StartDate <= currentDate)
                {
                    return "Pending Completion";
                }
                else if (StartDate.Value.Date == currentDate.Date)
                {
                    return "Today";
                }
                else
                {
                    int daysRemaining = (StartDate.Value.Date - currentDate.Date).Days;
                    return $"Due in {daysRemaining} day/s";
                }
            }
            else if (EndDate != null)
            {
                if (EndDate <= currentDate)
                {
                    return "Pending Completion";
                }
                else
                {
                    int daysRemaining = (EndDate.Value.Date - currentDate.Date).Days;
                    return $"Ends in {daysRemaining} day/s";
                }
            }
            else
            {
                return "";
            }
        }        
    }
}
