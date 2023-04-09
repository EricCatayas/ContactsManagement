using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactLogsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.ContactsManager.ContactLogs
{
    public class ContactLogsGetterService : IContactLogsGetterService
    {
        private readonly IContactLogsGetterRepository _contactLogsGetterRepository;

        public ContactLogsGetterService(IContactLogsGetterRepository contactLogsGetterRepository)
        {
            _contactLogsGetterRepository = contactLogsGetterRepository;
        }
        public async Task<ContactLogResponse?> GetContactLogById(int contactLogId, Guid userId)
        {
            ContactLog contactLog = await _contactLogsGetterRepository.GetContactLog(contactLogId, userId);
            return contactLog == null ? null : contactLog.ToContactLogResponse();
        }

        public async Task<List<ContactLogResponse>?> GetContactLogs(Guid userId)
        {
            List<ContactLog>? contactLogs = await _contactLogsGetterRepository.GetContactLogs(userId);
            return contactLogs.Select(log => log.ToContactLogResponse()).ToList();
        }

        public async Task<List<ContactLogResponse>?> GetContactLogsFromPerson(Guid personId)
        {
            List<ContactLog>? contactLogsFromPerson = await _contactLogsGetterRepository.GetContactLogsFromPerson(personId);
            return contactLogsFromPerson.Select(log => log.ToContactLogResponse()).ToList();
        }

        public List<ContactLogResponse>? GetFilteredContactLogs(List<ContactLogResponse>? contactLogs, string? searchText)
        {
            if (searchText == null || contactLogs == null)
            {
                return contactLogs;
            }
            try
            {
                contactLogs = FilterLogs(contactLogs, log =>
                    log.PersonLog.Contains(searchText, StringComparison.OrdinalIgnoreCase) || log.LogTitle != null && log.LogTitle.Contains(searchText, StringComparison.OrdinalIgnoreCase) || log.Note != null && log.Note.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                );
                return contactLogs;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public static List<ContactLogResponse>? FilterLogs(List<ContactLogResponse> contactLogs, Expression<Func<ContactLogResponse, bool>> predicate)
        {
            return  contactLogs.Where(predicate.Compile()).ToList();
        }
    }
}
