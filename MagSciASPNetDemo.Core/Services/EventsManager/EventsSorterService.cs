using ContactsManagement.Core.DTO.EventsManager;
using ContactsManagement.Core.Enums.ContactsManager;
using ContactsManagement.Core.ServiceContracts.EventsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.EventsManager
{
    public class EventsSorterService : IEventsSorterService
    {
        public List<EventResponse>? GetSortedEvents(List<EventResponse> events, string? sortby, SortOrderOptions sortOrder)
        {
            if (events == null || events.Count == 0)
                return null;
            List<EventResponse> sortedEvents = new List<EventResponse>();
            
            throw new NotImplementedException();
        }
    }
}
