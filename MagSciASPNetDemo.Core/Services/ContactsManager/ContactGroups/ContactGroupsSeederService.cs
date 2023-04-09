using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactGroupsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.ContactsManager.ContactGroups
{
    public class ContactGroupsSeederService : IContactGroupsSeederService
    {
        private readonly IContactGroupsAdderRepository _contactGroupsAdderRepository;

        public ContactGroupsSeederService(IContactGroupsAdderRepository contactGroupsAdderRepository)
        {
            _contactGroupsAdderRepository = contactGroupsAdderRepository;
        }
        public async Task SeedUserContactGroups(Guid userId)
        {
            ContactGroup friendsGroup = new ContactGroup() { GroupName = "Friends", Description = "Friends contacts", UserId = userId };
            ContactGroup workGroup = new ContactGroup() { GroupName = "Work", Description = "Work contacts", UserId = userId };
            ContactGroup familyGroup = new ContactGroup() { GroupName = "Family", Description = "Family contacts", UserId = userId };
            ContactGroup servicesGroup = new ContactGroup() { GroupName = "Services", Description = "Services contacts", UserId = userId };
            
            await _contactGroupsAdderRepository.AddContactGroup(friendsGroup, null);
            await _contactGroupsAdderRepository.AddContactGroup(workGroup, null);
            await _contactGroupsAdderRepository.AddContactGroup(familyGroup, null);
            await _contactGroupsAdderRepository.AddContactGroup(servicesGroup, null);
        }
    }
}
