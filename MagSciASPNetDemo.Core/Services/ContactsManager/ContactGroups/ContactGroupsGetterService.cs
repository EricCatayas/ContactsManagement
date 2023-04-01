using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactGroupsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.ContactsManager.ContactGroups
{
    public class ContactGroupsGetterService : IContactGroupsGetterService
    {
        private readonly IContactGroupsGetterRepository _contactGroupsGetterRepository;

        public ContactGroupsGetterService(IContactGroupsGetterRepository contactGroupsRepository)
        {
            _contactGroupsGetterRepository = contactGroupsRepository;
        }
        public async Task<List<PersonResponse>?> GetAllContactGroupPersons(int contactGroupId)
        {
            ContactGroup? contactGroup = await _contactGroupsGetterRepository.GetContactGroupById(contactGroupId);

            if (contactGroup == null) return null;
            List<Person>? persons = contactGroup.Persons.ToList();

            return persons != null ? persons?.Select(person => person.ToPersonResponse()).ToList() : null;
        }

        public async Task<List<ContactGroupResponse>?> GetAllContactGroups()
        {
            List<ContactGroup>? contactGroups = await _contactGroupsGetterRepository.GetContactGroups();
            return contactGroups != null ? contactGroups.Select(group => group.ToContactGroupResponse()).ToList() : null;
        }

        public async Task<ContactGroupResponse?> GetContactGroupById(int contactGroupId)
        {
            ContactGroup? contactGroup = await _contactGroupsGetterRepository.GetContactGroupById(contactGroupId);
            return contactGroup != null ? contactGroup.ToContactGroupResponse() : null;
        }
    }
}
