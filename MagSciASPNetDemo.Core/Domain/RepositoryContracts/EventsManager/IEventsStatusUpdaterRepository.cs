using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.RepositoryContracts.EventsManager
{
    public interface IEventsStatusUpdaterRepository
    {
        Task<Event?> UpdateEventStatus(Event? @event);
        Task<List<Event>?> UpdateEventsStatus(List<Event>? events);
        string GetUpdatedStatus(DateTime? StartDate, DateTime? EndDate);
    }
}
