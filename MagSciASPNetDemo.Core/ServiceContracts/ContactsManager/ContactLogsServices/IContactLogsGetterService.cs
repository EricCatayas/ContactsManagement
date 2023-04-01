using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.ContactLogsServices
{
    public interface IContactLogsGetterService
    {
        public Task<ContactLogResponse?> GetContactLogById(int contactLogId);
        public Task<List<ContactLogResponse>?> GetContactLogs();
        public Task<List<ContactLogResponse>?> GetContactLogsFromPerson(Guid personId);
        /// <summary>
        /// Searches if Log Title, Note, Person or Type contains matching Text
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public List<ContactLogResponse>? GetFilteredContactLogs(List<ContactLogResponse>? contactLogs, string? searchText);
    }
}
