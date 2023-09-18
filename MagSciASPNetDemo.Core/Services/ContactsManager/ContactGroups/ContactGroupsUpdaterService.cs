using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.Exceptions;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactGroupsServices;

namespace ContactsManagement.Core.Services.ContactsManager.ContactGroups
{
    public class ContactGroupsUpdaterService : IContactGroupsUpdaterService
    {
        private readonly IContactGroupsUpdaterRepository _contactGroupsUpdaterRepository;
        private readonly IPersonsGetterRepository _personsGetterRepository;
        private readonly ISignedInUserService _signedInUserService;

        public ContactGroupsUpdaterService(IContactGroupsUpdaterRepository contactGroupsUpdaterRepository, IPersonsGetterRepository personsGetterRepository, ISignedInUserService signedInUserService)
        {
            _contactGroupsUpdaterRepository = contactGroupsUpdaterRepository;
            _personsGetterRepository = personsGetterRepository;
            _signedInUserService = signedInUserService;
        }
        public async Task<ContactGroupResponse?> UpdateContactGroup(ContactGroupUpdateRequest contactGroupUpdateRequest)
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();

            ContactGroup contactGroup = contactGroupUpdateRequest.ToContactGroup();

            contactGroup.UserId = userId;
            contactGroup.Persons = await _personsGetterRepository.GetPersonsById(contactGroupUpdateRequest.Persons);

            contactGroup = await _contactGroupsUpdaterRepository.UpdateContactGroup(contactGroup);
            return contactGroup.ToContactGroupResponse();
        }
    }
}
