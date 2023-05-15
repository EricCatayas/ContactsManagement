using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.Exceptions;
using ContactsManagement.Core.ServiceContracts.AccountManager;
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
        private readonly IContactLogsGetterRepository _contactLogsGetterRepository;
        private readonly ISignedInUserService _signedInUserService;

        public ContactLogsDeleterService(IContactLogsDeleterRepository contactLogsDeleterRepository, IContactLogsGetterRepository contactLogsGetterRepository, ISignedInUserService signedInUserService)
        {
            _contactLogsDeleterRepository = contactLogsDeleterRepository;
            _contactLogsGetterRepository = contactLogsGetterRepository;
            _signedInUserService = signedInUserService;
        }
        public async Task<bool> DeleteContactLog(int contactLogId)
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();

            ContactLog? contactLog = await _contactLogsGetterRepository.GetContactLog(contactLogId, (Guid)userId);
            if (contactLog == null)
                return false;
            return await _contactLogsDeleterRepository.DeleteContactLog(contactLog);
        }
    }
}
