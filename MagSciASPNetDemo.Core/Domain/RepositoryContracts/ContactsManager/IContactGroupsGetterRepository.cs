using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager
{
    public interface IContactGroupsGetterRepository
    {
        Task<ContactGroup?> GetContactGroupById(int contactGroupId, Guid userId);
        Task<List<ContactGroup>> GetContactGroups(Guid userId);
        Task<List<ContactGroup>> GetContactGroupsById(List<int>? contactGroupIds);

    }
}
