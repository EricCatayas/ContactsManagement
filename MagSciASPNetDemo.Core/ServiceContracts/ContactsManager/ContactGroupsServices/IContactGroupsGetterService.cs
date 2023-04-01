using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.ContactGroupsServices
{
    public interface IContactGroupsGetterService
    {
        Task<List<PersonResponse>?> GetAllContactGroupPersons(int contactGroupId);
        Task<List<ContactGroupResponse>?> GetAllContactGroups();
        Task<ContactGroupResponse?> GetContactGroupById(int contactGroupId);
    }
}
