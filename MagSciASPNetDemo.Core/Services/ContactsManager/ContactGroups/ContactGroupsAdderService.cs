using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactGroupsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.ContactsManager.ContactGroups
{
    public class ContactGroupsAdderService : IContactGroupsAdderService
    {
        private readonly IContactGroupsAdderRepository _contactGroupsAdderRepository;

        public ContactGroupsAdderService(IContactGroupsAdderRepository contactGroupsAdderRepository)
        {
            _contactGroupsAdderRepository = contactGroupsAdderRepository;
        }
        public async Task<ContactGroupResponse> AddContactGroup(ContactGroupAddRequest contactGroup)
        {
            ContactGroup contactGroupAdded = await _contactGroupsAdderRepository.AddContactGroup(contactGroup.ToContactGroup(), contactGroup.Persons);
            return contactGroupAdded.ToContactGroupResponse();
        }
    }
}
