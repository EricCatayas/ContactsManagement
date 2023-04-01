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
    public class ContactGroupsUpdaterService : IContactGroupsUpdaterService
    {
        private readonly IContactGroupsUpdaterRepository _contactGroupsUpdaterRepository;
        private readonly IPersonsGetterRepository _personsGetterRepository;

        public ContactGroupsUpdaterService(IContactGroupsUpdaterRepository contactGroupsUpdaterRepository, IPersonsGetterRepository personsGetterRepository)
        {
            _contactGroupsUpdaterRepository = contactGroupsUpdaterRepository;
            _personsGetterRepository = personsGetterRepository;
        }
        public async Task<ContactGroupResponse?> UpdateContactGroup(ContactGroupUpdateRequest contactGroupUpdateRequest)
        {
            ContactGroup contactGroup = contactGroupUpdateRequest.ToContactGroup();
            contactGroup.Persons = await _personsGetterRepository.GetPersonsById(contactGroupUpdateRequest.Persons);
            contactGroup = await _contactGroupsUpdaterRepository.UpdateContactGroup(contactGroup);
            return contactGroup.ToContactGroupResponse();
        }
    }
}
