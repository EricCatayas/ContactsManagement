using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
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
    public class ContactGroupsAdderService : IContactGroupsAdderService
    {
        private readonly IContactGroupsAdderRepository _contactGroupsAdderRepository;
        private readonly ISignedInUserService _signedInUserService;
        private Guid? _userId;

        public ContactGroupsAdderService(IContactGroupsAdderRepository contactGroupsAdderRepository, ISignedInUserService signedInUserService)
        {
            _contactGroupsAdderRepository = contactGroupsAdderRepository;
            _signedInUserService = signedInUserService;
        }
        public async Task<ContactGroupResponse> AddContactGroup(ContactGroupAddRequest contactGroupAddRequest)
        {
            var isSignedIn = IsRequestSignedIn();
            if (!isSignedIn)
                throw new AccessDeniedException();

            ContactGroup contactGroup = contactGroupAddRequest.ToContactGroup();
            contactGroup.UserId = _userId;

            contactGroup = await _contactGroupsAdderRepository.AddContactGroup(contactGroup, contactGroupAddRequest.Persons);
            return contactGroup.ToContactGroupResponse();
        }
        private bool IsRequestSignedIn()
        {
            _userId = _signedInUserService.GetSignedInUserId();

            return _userId == null ? false : true;
        }
    }
}
