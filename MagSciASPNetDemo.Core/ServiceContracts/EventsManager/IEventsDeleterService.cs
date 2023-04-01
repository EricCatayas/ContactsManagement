using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.EventsManager
{
    public interface IEventsDeleterService
    {
        Task<bool> DeleteEvent(int eventId);
    }
}
