using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactLogsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.ContactsManager.ContactLogs
{
    public class ContactLogsDeleterService : IContactLogsDeleterService
    {
        private readonly IContactLogsDeleterRepository _contactLogsDeleterRepository;

        public ContactLogsDeleterService(IContactLogsDeleterRepository contactLogsDeleterRepository)
        {
            _contactLogsDeleterRepository = contactLogsDeleterRepository;
        }
        public async Task<bool> DeleteContactLog(int ContactLogId)
        {
            return await _contactLogsDeleterRepository.DeleteContactLog(ContactLogId);
        }
    }
}
