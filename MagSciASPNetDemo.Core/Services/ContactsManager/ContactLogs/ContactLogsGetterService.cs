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
    public class ContactLogsGetterService : IContactLogsGetterService
    {
        private readonly IContactLogsGetterRepository _contactLogsGetterRepository;
        private readonly ISignedInUserService _signedInUserService;

        public ContactLogsGetterService(IContactLogsGetterRepository contactLogsGetterRepository, ISignedInUserService signedInUserService)
        {
            _contactLogsGetterRepository = contactLogsGetterRepository;
            _signedInUserService = signedInUserService;
        }
        public async Task<ContactLogResponse?> GetContactLogById(int contactLogId)
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();

            ContactLog? contactLog = await _contactLogsGetterRepository.GetContactLog(contactLogId, (Guid)userId);
            return contactLog == null ? null : contactLog.ToContactLogResponse();
        }

        public async Task<List<ContactLogResponse>?> GetContactLogs()
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();

            List<ContactLog>? contactLogs = await _contactLogsGetterRepository.GetContactLogs((Guid)userId);
            return contactLogs.Select(log => log.ToContactLogResponse()).ToList();
        }

        public async Task<List<ContactLogResponse>?> GetContactLogsFromPerson(Guid personId)
        {
            List<ContactLog>? contactLogsFromPerson = await _contactLogsGetterRepository.GetContactLogsFromPerson(personId);
            return contactLogsFromPerson.Select(log => log.ToContactLogResponse()).ToList();
        }

        public async Task<List<ContactLogResponse>?> GetFilteredContactLogs(string? searchText)
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();

            List<ContactLog> contactLogs = new List<ContactLog>();
            try
            {
                if (searchText != null)
                {
                    contactLogs = await _contactLogsGetterRepository.GetFilteredContactLogs(log =>
                    log.PersonLog.Name.Contains(searchText) || log.LogTitle != null && log.LogTitle.Contains(searchText) || log.Note != null && log.Note.Contains(searchText), (Guid)userId);
                }
                else
                {
                    return await this.GetContactLogs();
                }
                return contactLogs.Select(temp => temp.ToContactLogResponse()).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
