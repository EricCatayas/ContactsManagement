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
    public class ContactLogsUpdaterService : IContactLogsUpdaterService
    {
        private readonly IContactLogsUpdaterRepository _contactLogsUpdaterRepository;

        public ContactLogsUpdaterService(IContactLogsUpdaterRepository contactLogsUpdaterRepository)
        {
            _contactLogsUpdaterRepository = contactLogsUpdaterRepository;
        }
        public async Task<ContactLogResponse> UpdateContactLog(ContactLogUpdateRequest contactLogUpdateRequest)
        {
            ContactLog contactLog =  await _contactLogsUpdaterRepository.UpdateContactLog(contactLogUpdateRequest.ToContactLog());
            return contactLog.ToContactLogResponse();

        }
    }
}
