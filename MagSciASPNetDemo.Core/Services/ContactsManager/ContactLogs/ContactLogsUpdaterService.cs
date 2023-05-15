using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
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
    public class ContactLogsUpdaterService : IContactLogsUpdaterService
    {
        private readonly IContactLogsUpdaterRepository _contactLogsUpdaterRepository;
        private readonly ISignedInUserService _signedInUserService;

        public ContactLogsUpdaterService(IContactLogsUpdaterRepository contactLogsUpdaterRepository, ISignedInUserService signedInUserService)
        {
            _contactLogsUpdaterRepository = contactLogsUpdaterRepository;
            _signedInUserService = signedInUserService;
        }
        public async Task<ContactLogResponse> UpdateContactLog(ContactLogUpdateRequest contactLogUpdateRequest)
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();

            ContactLog contactLog_ToUpdate = contactLogUpdateRequest.ToContactLog();
            contactLog_ToUpdate.UserId = userId;
            ContactLog contactLog =  await _contactLogsUpdaterRepository.UpdateContactLog(contactLog_ToUpdate);
            return contactLog.ToContactLogResponse();

        }
    }
}
