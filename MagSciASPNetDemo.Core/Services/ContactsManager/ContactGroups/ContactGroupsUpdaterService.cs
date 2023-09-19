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
        private Guid? _userId;
        private ContactGroup? _contactGroupToUpdate;
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
            if (!IsRequestSignedIn())
                throw new AccessDeniedException();

            await CreateContactGroupToUpdate(contactGroupUpdateRequest);            

            var updatedContactGroup = await _contactGroupsUpdaterRepository.UpdateContactGroup(_contactGroupToUpdate);
            return updatedContactGroup.ToContactGroupResponse();
        }

        private async Task CreateContactGroupToUpdate(ContactGroupUpdateRequest contactGroupUpdateRequest)
        {
            _contactGroupToUpdate = contactGroupUpdateRequest.ToContactGroup();
            _contactGroupToUpdate.UserId = _userId;
            _contactGroupToUpdate.Persons = await _personsGetterRepository.GetPersonsById(contactGroupUpdateRequest.Persons);
        }
        private bool IsRequestSignedIn()
        {
            _userId = _signedInUserService.GetSignedInUserId();

            return _userId == null ? false : true;
        }
    }
}
