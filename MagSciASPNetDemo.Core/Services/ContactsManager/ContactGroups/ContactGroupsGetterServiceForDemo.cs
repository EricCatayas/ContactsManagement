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
        private Guid _userId;
        private readonly IContactGroupsGetterRepository _contactGroupsGetterRepository;
        private readonly ISignedInUserService _signedInUserService;
        private readonly IDemoUserService _demoUserService;

        public ContactGroupsGetterServiceForDemo(IContactGroupsGetterRepository contactGroupsRepository, ISignedInUserService signedInUserService, IDemoUserService demoUserService)
        {
            _contactGroupsGetterRepository = contactGroupsRepository;
            _signedInUserService = signedInUserService;
            _demoUserService = demoUserService;
        }
        public async Task<List<PersonResponse>> GetAllContactGroupPersons(int contactGroupId)
        {
            
            if (!IsUserSignedIn())
                throw new AccessDeniedException();

            ContactGroup? contactGroup = await _contactGroupsGetterRepository.GetContactGroupById(contactGroupId, _userId);

            List<PersonResponse> personsFromContactGroup = new List<PersonResponse>();

            if (contactGroup == null) 
                return personsFromContactGroup;

            personsFromContactGroup = contactGroup.Persons.ToList();

            return personsFromContactGroup != null ? personsFromContactGroup?.Select(person => person.ToPersonResponse()).ToList() : null;
        }

        public async Task<List<ContactGroupResponse>> GetAllContactGroups()
        {
            if (!IsUserSignedIn())
                UseDemoUserId();

            List<ContactGroup> contactGroups = await _contactGroupsGetterRepository.GetContactGroups(_userId);

            return contactGroups.Count > 0 ? 
                contactGroups.Select(group => group.ToContactGroupResponse()).ToList() : 
                new List<ContactGroupResponse>();
        }

        public async Task<ContactGroupResponse?> GetContactGroupById(int contactGroupId)
        {
            if (!IsUserSignedIn())
                throw new AccessDeniedException();

            ContactGroup? contactGroup = await _contactGroupsGetterRepository.GetContactGroupById(contactGroupId, _userId);
            return contactGroup != null ? contactGroup.ToContactGroupResponse() : null;
        }
        private bool IsUserSignedIn()
        {
            var userId = _signedInUserService.GetSignedInUserId();

            if(userId != null)
            {
                _userId = (Guid)userId;
                return true;
            }
            else
            {
                return false;
            }
        }
        private void UseDemoUserId()
        {
            _userId = _demoUserService.GetDemoUserId();
        }
    }
}
