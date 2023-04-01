using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
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

        public ContactTagsAdderService(IContactTagsAdderRepository contactTagsAdderRepository)
        {
            _contactTagsAdderRepository = contactTagsAdderRepository;
        }
        public async Task<ContactTagDTO> AddContactTag(ContactTagAddRequest contactTag)
        {
            ContactTag contactTagResponse = await _contactTagsAdderRepository.AddContactTag(contactTag.ToContactTag());
            return contactTagResponse.ToContactTagResponse();
        }
    }
}
