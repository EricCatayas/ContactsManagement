using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager
{
    public interface IContactGroupsDeleterRepository
    {
        Task<bool> DeleteContactGroup(int contactGroupId, Guid userId);
    }
}
