using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.Exceptions;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactTagsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.ContactsManager.ContactTags
{
    public class ContactTagsAdderService : IContactTagsAdderService
    {
        private readonly IContactTagsAdderRepository _contactTagsAdderRepository;
        private readonly ISignedInUserService _signedInUserService;

        public ContactTagsAdderService(IContactTagsAdderRepository contactTagsAdderRepository, ISignedInUserService signedInUserService)
        {
            _contactTagsAdderRepository = contactTagsAdderRepository;
            _signedInUserService = signedInUserService;
        }
        public async Task<ContactTagDTO> AddContactTag(ContactTagAddRequest contactTagAddRequest)
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();

            ContactTag contactTag = contactTagAddRequest.ToContactTag();
            contactTag.UserId = userId;
            contactTag = await _contactTagsAdderRepository.AddContactTag(contactTag);
            return contactTag.ToContactTagResponse();
        }
    }
}
