using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
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
    public class ContactTagsDeleterService : IContactTagsDeleterService
    {
        private readonly IContactTagsDeleterRepository _contactTagsDeleterRepository;
        private readonly ISignedInUserService _signedInUserService;

        public ContactTagsDeleterService(IContactTagsDeleterRepository contactTagsDeleterRepository, ISignedInUserService signedInUserService)
        {
            _contactTagsDeleterRepository = contactTagsDeleterRepository;
            _signedInUserService = signedInUserService;
        }
        public async Task<bool> DeleteContactTag(int contactTagId)
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();

            return await _contactTagsDeleterRepository.DeleteContactTagById(contactTagId, (Guid)userId);  
        }
    }
}
