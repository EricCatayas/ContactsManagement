using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.Enums.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.ContactLogsServices
{
    public interface IContactLogsSorterService
    {
        public List<ContactLogResponse>? GetSortedContactLogs(List<ContactLogResponse>? contactLogs, string sortBy, SortOrderOptions sortOption);
    }
}
