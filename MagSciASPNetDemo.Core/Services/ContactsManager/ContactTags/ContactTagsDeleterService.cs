using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactTagsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.ContactsManager.ContactTags
{
    public class ContactTagsDeleterService : IContactTagsDeleterService
    {
        private readonly IContactTagsDeleterRepository _contactTagsDeleterRepository;

        public ContactTagsDeleterService(IContactTagsDeleterRepository contactTagsDeleterRepository)
        {
            _contactTagsDeleterRepository = contactTagsDeleterRepository;
        }
        public async Task<bool> DeleteContactTag(int contactTagId)
        {
            return await _contactTagsDeleterRepository.DeleteContactTagById(contactTagId);  
        }
    }
}
