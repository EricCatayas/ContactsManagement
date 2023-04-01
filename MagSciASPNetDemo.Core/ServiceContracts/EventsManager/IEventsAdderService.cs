using ContactsManagement.Core.DTO.EventsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.EventsManager
{
    public interface IEventsAdderService
    {
        Task<EventResponse> AddEvent(EventAddRequest eventAddRequest);
    }
}
