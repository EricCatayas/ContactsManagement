using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.ContactLogsServices
{
    public interface IContactLogsAdderService
    {
        public Task<ContactLogResponse> AddContactLog(ContactLogAddRequest contactLogAddRequest);
    }
}
