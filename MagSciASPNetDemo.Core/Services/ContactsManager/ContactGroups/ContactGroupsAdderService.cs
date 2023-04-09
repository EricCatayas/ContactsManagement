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
        public async Task<ContactGroupResponse> AddContactGroup(ContactGroupAddRequest contactGroupAddRequest, Guid userId)
        {
            ContactGroup contactGroup = contactGroupAddRequest.ToContactGroup();
            contactGroup.UserId = userId;
            contactGroup = await _contactGroupsAdderRepository.AddContactGroup(contactGroup, contactGroupAddRequest.Persons);
            return contactGroup.ToContactGroupResponse();
        }
    }
}
