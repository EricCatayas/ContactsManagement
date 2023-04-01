using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactLogsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager
{
    public interface IContactLogsService : IContactLogsAdderService, IContactLogsGetterService, IContactLogsUpdaterService, IContactLogsDeleterService
    {        
    }
}
