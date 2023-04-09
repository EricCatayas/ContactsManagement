using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.Enums.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.ContactLogsServices
{
    /// <summary>
    /// Defines a service that manages sorting of contact logs.
    /// </summary>
    public interface IContactLogsSorterService
    {
        /// <summary>
        /// Gets a sorted list of contact logs.
        /// </summary>
        /// <param name="contactLogs">The list of contact logs to sort.</param>
        /// <param name="sortBy">The property to sort by (e.g. "PersonLog", "LogTitle", "DateCreated").</param>
        /// <param name="sortOption">The sort order to use (ascending or descending).</param>
        /// <returns>A sorted list of contact logs.</returns>
        public List<ContactLogResponse>? GetSortedContactLogs(List<ContactLogResponse>? contactLogs, string sortBy, SortOrderOptions sortOption);
    }
}
