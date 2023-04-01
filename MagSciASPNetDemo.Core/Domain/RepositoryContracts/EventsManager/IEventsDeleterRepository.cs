using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.RepositoryContracts.EventsManager
{
    public interface IEventsDeleterRepository
    {
        Task<bool> DeleteEvent(int eventId);
    }
}
