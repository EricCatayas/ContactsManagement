using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.ContactLogsServices
{
    public interface IContactLogsUpdaterService
    {
        public Task<ContactLogResponse> UpdateContactLog(ContactLogUpdateRequest contactLogAddRequest);
    }
}
