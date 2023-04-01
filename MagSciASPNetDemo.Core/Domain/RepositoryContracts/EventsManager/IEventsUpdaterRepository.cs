using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.RepositoryContracts.EventsManager
{
    public interface IEventsUpdaterRepository
    {
        public Task<bool> UpdateEvent(Event @event);
    }
}
