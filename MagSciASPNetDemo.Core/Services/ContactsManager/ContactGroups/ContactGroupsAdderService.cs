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
        private readonly IPersonsGetterRepository _personsGetterRepository;
        private readonly ISignedInUserService _signedInUserService;
        private Guid? _userId;
        private ContactGroup? _contactGroupToAdd;

        public ContactGroupsAdderService(IContactGroupsAdderRepository contactGroupsAdderRepository, IPersonsGetterRepository personsGetterRepository, ISignedInUserService signedInUserService)
        {
            _contactGroupsAdderRepository = contactGroupsAdderRepository;
            _personsGetterRepository = personsGetterRepository;
            _signedInUserService = signedInUserService;
        }
        public async Task<ContactGroupResponse> AddContactGroup(ContactGroupAddRequest contactGroupAddRequest)
        {
            if (!IsRequestSignedIn())
                throw new AccessDeniedException();

            await CreateContactGroupToAdd(contactGroupAddRequest);

            _contactGroupToAdd = await _contactGroupsAdderRepository.AddContactGroup(_contactGroupToAdd);
            return _contactGroupToAdd.ToContactGroupResponse();
        }
        private async Task CreateContactGroupToAdd(ContactGroupAddRequest contactGroupAddRequest)
        {
            _contactGroupToAdd = contactGroupAddRequest.ToContactGroup();
            _contactGroupToAdd.UserId = _userId;
            await GetPersonsForContactGroup(contactGroupAddRequest.Persons);
        }

        private async Task GetPersonsForContactGroup(List<Guid>? persons)
        {
            _contactGroupToAdd!.Persons = await _personsGetterRepository.GetPersonsById(persons);
        }

        private bool IsRequestSignedIn()
        {
            _userId = _signedInUserService.GetSignedInUserId();

            return _userId == null ? false : true;
        }
    }
}
