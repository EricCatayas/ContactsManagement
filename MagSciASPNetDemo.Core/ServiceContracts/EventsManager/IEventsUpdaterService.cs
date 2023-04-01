using ContactsManagement.Core.DTO.EventsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.EventsManager
{
    public interface IEventsUpdaterService
    {
        Task<bool> UpdateEvent(EventUpdateRequest eventUpdateRequest);
        Task<bool> UpdateEventCompletion(int  eventId);
    }
}
