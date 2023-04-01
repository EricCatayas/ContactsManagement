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
    public class ContactTagsGetterService : IContactTagsGetterService
    {
        private readonly IContactTagsGetterRepository _contactTagsGetterRepository;

        public ContactTagsGetterService(IContactTagsGetterRepository contactTagsGetterRepository)
        {
            _contactTagsGetterRepository = contactTagsGetterRepository;
        }
        public async Task<List<ContactTagDTO>?> GetAllContactTags()
        {
            List<ContactTag>? contactTags = await _contactTagsGetterRepository.GetAllContactTags();
            return contactTags?.Select(tag => tag.ToContactTagResponse()).ToList();
        }
    }
}
