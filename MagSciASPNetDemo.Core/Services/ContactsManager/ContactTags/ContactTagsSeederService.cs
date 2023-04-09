using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactTagsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.ContactsManager.ContactTags
{
    public class ContactTagsSeederService : IContactTagsSeederService
    {
        private readonly IContactTagsAdderRepository _contactTagsAdderRepository;

        public ContactTagsSeederService(IContactTagsAdderRepository contactTagsAdderRepository)
        {
            _contactTagsAdderRepository = contactTagsAdderRepository;
        }
        public async Task SeedUserContactTags(Guid userId)
        {
			/* The reason why the exception occurred when you iterated over the List<ContactTag>() was that you were calling AddContactTag() method for each ContactTag asynchronously and each call was not waiting for the previous one to complete. Therefore, multiple calls to AddContactTag() were being made simultaneously and they were all using the same instance of the _db context. This caused a race condition where the SaveChangesAsync() method was being called for multiple entities at the same time, resulting in the System.InvalidOperationException exception. */
			ContactTag urgentTag = new ContactTag() { TagName = "Urgent", TagColor = "#FF0000", UserId = userId };
            ContactTag reminderTag = new ContactTag() { TagName = "Reminder", TagColor = "#FFFF00", UserId = userId };
            ContactTag newTag = new ContactTag() { TagName = "New", TagColor = "#00FF00", UserId = userId };
            ContactTag incompleteTag = new ContactTag() { TagName = "Incomplete", TagColor = "#808080", UserId = userId };

            await _contactTagsAdderRepository.AddContactTag(urgentTag);
            await _contactTagsAdderRepository.AddContactTag(reminderTag);
            await _contactTagsAdderRepository.AddContactTag(newTag);
            await _contactTagsAdderRepository.AddContactTag(incompleteTag);            
        }
    }
}
