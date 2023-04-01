using ContactsManagement.Core.DTO.EventsManager;
using ContactsManagement.Core.Enums.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.EventsManager
{
    public interface IEventsSorterService
    {
        List<EventResponse>? GetSortedEvents(List<EventResponse>? events, string? sortby, SortOrderOptions sortOrder);
    }
}
