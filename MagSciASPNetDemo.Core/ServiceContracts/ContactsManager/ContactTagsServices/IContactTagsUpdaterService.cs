using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.ContactTagsServices
{
    public interface IContactTagsUpdaterService
    {
        Task<bool> RemoveContactTagFromPerson(Guid personId);
    }
}
