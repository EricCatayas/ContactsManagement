using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
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

        public ContactGroupsDeleterService(IContactGroupsDeleterRepository contactGroupsDeleterRepository)
        {
            _contactGroupsDeleterRepository = contactGroupsDeleterRepository;
        }
        public async Task<bool> DeleteContactGroup(int contactGroupId, Guid userId)
        {
            return await _contactGroupsDeleterRepository.DeleteContactGroup(contactGroupId, userId);
        }
    }
}
