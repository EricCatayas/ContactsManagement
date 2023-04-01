using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactLogsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.ContactsManager.ContactLogs
{
    public class ContactLogsAdderService : IContactLogsAdderService
    {
        private readonly IContactLogsAdderRepository _contactLogsAdderRepository;

        public ContactLogsAdderService(IContactLogsAdderRepository contactLogsAdderRepository)
        {
            _contactLogsAdderRepository = contactLogsAdderRepository;
        }
        public async Task<ContactLogResponse> AddContactLog(ContactLogAddRequest contactLogAddRequest)
        {
            ContactLog contactLog = await _contactLogsAdderRepository.AddContactLog(contactLogAddRequest.ToContactLog());
            return contactLog.ToContactLogResponse();
        }
    }
}
