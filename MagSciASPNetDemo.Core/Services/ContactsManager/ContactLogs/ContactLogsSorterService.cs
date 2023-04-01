using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.Enums.ContactsManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactLogsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.ContactsManager.ContactLogs
{
    public class ContactLogsSorterService : IContactLogsSorterService
    {
        public List<ContactLogResponse>? GetSortedContactLogs(List<ContactLogResponse>? contactLogs, string sortBy, SortOrderOptions sortOption)
        {
            if (contactLogs == null) return contactLogs;
            List<ContactLogResponse> sortedContactLogs = new List<ContactLogResponse>();
            if (sortOption.CompareTo(SortOrderOptions.ASC) == 0) // if ASC
            {
                switch(sortBy)
                {
                    case nameof(ContactLogResponse.PersonLog):
                        sortedContactLogs = contactLogs.OrderBy(log => log.PersonLog).ToList();
                        break;
                    case nameof(ContactLogResponse.LogTitle):
                        sortedContactLogs = contactLogs.OrderBy(log => log.LogTitle).ToList();
                        break;
                    case nameof(ContactLogResponse.DateCreated):
                        sortedContactLogs = contactLogs.OrderBy(log => log.DateCreated).ToList();
                        break;
                }
            }
            else
            {
                switch (sortBy)
                {
                    case nameof(ContactLogResponse.PersonLog):
                        sortedContactLogs = contactLogs.OrderByDescending(log => log.PersonLog).ToList();
                        break;
                    case nameof(ContactLogResponse.LogTitle):
                        sortedContactLogs = contactLogs.OrderByDescending(log => log.LogTitle).ToList();
                        break;
                    case nameof(ContactLogResponse.DateCreated):
                        sortedContactLogs = contactLogs.OrderByDescending(log => log.DateCreated).ToList();
                        break;
                }
            }
            return sortedContactLogs;
        }
    }
}
