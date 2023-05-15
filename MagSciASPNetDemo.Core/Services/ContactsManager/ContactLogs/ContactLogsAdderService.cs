using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.Exceptions;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactLogsServices;
using ContactsManagement.Core.Services.AccountManager;
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
        private readonly ISignedInUserService _signedInUserService;

        public ContactLogsAdderService(IContactLogsAdderRepository contactLogsAdderRepository, ISignedInUserService signedInUserService)
        {
            _contactLogsAdderRepository = contactLogsAdderRepository;
            _signedInUserService = signedInUserService;
        }
        public async Task<ContactLogResponse> AddContactLog(ContactLogAddRequest contactLogAddRequest)
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();

            if (contactLogAddRequest.PersonId == null)
            {
                throw new ArgumentException("Contact Log must contain Person Id");
            }
            ContactLog contactLog = contactLogAddRequest.ToContactLog();
            contactLog.UserId = userId;
            contactLog = await _contactLogsAdderRepository.AddContactLog(contactLog);
            return contactLog.ToContactLogResponse();
        }
    }
}
