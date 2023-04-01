using ContactsManagement.Core.DTO.EventsManager;
using ContactsManagement.Core.Enums.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.EventsManager
{
    public interface IEventsGetterService
    {
        Task<List<EventResponse>?> GetEvents();
        Task<EventResponse?> GetEventById(int eventId);
        List<EventResponse>? GetFilteredEvents(List<EventResponse>? events, StatusType statusType);
    }
}
