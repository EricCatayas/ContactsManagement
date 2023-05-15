using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
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
    public class ContactGroupsDeleterService : IContactGroupsDeleterService
    {
        private readonly IContactGroupsDeleterRepository _contactGroupsDeleterRepository;
        private readonly ISignedInUserService _signedInUserService;

        public ContactGroupsDeleterService(IContactGroupsDeleterRepository contactGroupsDeleterRepository, ISignedInUserService signedInUserService)
        {
            _contactGroupsDeleterRepository = contactGroupsDeleterRepository;
            _signedInUserService = signedInUserService;
        }
        public async Task<bool> DeleteContactGroup(int contactGroupId)
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();

            return await _contactGroupsDeleterRepository.DeleteContactGroup(contactGroupId, (Guid)userId);
        }
    }
}
