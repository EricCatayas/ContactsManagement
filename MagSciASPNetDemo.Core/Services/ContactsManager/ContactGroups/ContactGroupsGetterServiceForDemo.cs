using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.Exceptions;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactGroupsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.ContactsManager.ContactGroups
{
    public class ContactGroupsGetterServiceForDemo : IContactGroupsGetterService
    {
        private readonly IContactGroupsGetterRepository _contactGroupsGetterRepository;
        private readonly ISignedInUserService _signedInUserService;
        private readonly IDemoUserService _demoUserService;

        public ContactGroupsGetterServiceForDemo(IContactGroupsGetterRepository contactGroupsRepository, ISignedInUserService signedInUserService, IDemoUserService demoUserService)
        {
            _contactGroupsGetterRepository = contactGroupsRepository;
            _signedInUserService = signedInUserService;
            _demoUserService = demoUserService;
        }
        public async Task<List<PersonResponse>?> GetAllContactGroupPersons(int contactGroupId)
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();

            ContactGroup? contactGroup = await _contactGroupsGetterRepository.GetContactGroupById(contactGroupId, (Guid)userId);

            if (contactGroup == null) return null;
            List<Person>? persons = contactGroup.Persons.ToList();

            return persons != null ? persons?.Select(person => person.ToPersonResponse()).ToList() : null;
        }

        public async Task<List<ContactGroupResponse>?> GetAllContactGroups()
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                userId = _demoUserService.GetDemoUserId();

            List<ContactGroup>? contactGroups = await _contactGroupsGetterRepository.GetContactGroups((Guid)userId);
            return contactGroups != null ? contactGroups.Select(group => group.ToContactGroupResponse()).ToList() : null;
        }

        public async Task<ContactGroupResponse?> GetContactGroupById(int contactGroupId)
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();

            ContactGroup? contactGroup = await _contactGroupsGetterRepository.GetContactGroupById(contactGroupId, (Guid)userId);
            return contactGroup != null ? contactGroup.ToContactGroupResponse() : null;
        }
    }
}
