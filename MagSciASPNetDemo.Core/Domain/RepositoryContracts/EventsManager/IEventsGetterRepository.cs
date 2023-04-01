using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.RepositoryContracts.EventsManager
{
    public interface IEventsGetterRepository
    {
        Task<List<Event>?> GetEvents();
        Task<Event?> GetEvent(int EventId);

    }
}
