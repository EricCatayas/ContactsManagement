using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.ContactLogsServices
{
    /// <summary>
    /// Defines a service for retrieving contact log data from the system.
    /// </summary>
    public interface IContactLogsGetterService
    {
        /// <summary>
        /// Retrieves contact log from the system.
        /// </summary>
        /// <param name="contactLogId">The request id of the contact log to be retrieved.</param>
        /// <param name="userId">The ID of the user who owns the data.</param>
        /// <returns>The contact log object with the corresponding ID and UserID</returns>
        public Task<ContactLogResponse?> GetContactLogById(int contactLogId, Guid userId);
        /// <summary>
        /// Retrieves list of contact log from the system.
        /// </summary>
        /// <param name="userId">The ID of the user who owns the data.</param>
        /// <returns>A list of contact log object with the corresponding UserID</returns>
        public Task<List<ContactLogResponse>?> GetContactLogs(Guid userId);
        /// <summary>
        /// Retrieves list of contact log of person from the system.
        /// </summary>
        /// <param name="personId">The ID of the person.</param>
        /// <returns>A list of contact log object from the corresponding person</returns>
        public Task<List<ContactLogResponse>?> GetContactLogsFromPerson(Guid personId);
        /// <summary>
        /// Filters a list of contact logs based on a search text.
        /// </summary>
        /// <param name="contactLogs">The list of contact logs to filter.</param>
        /// <param name="searchText">The search text to filter by.</param>
        /// <returns>A filtered list of contact logs.</returns>
        /// <remarks>
        /// The search is case-insensitive and looks for matches in the PersonLog, LogTitle, and Note properties of each contact log.
        /// </remarks>

        public List<ContactLogResponse>? GetFilteredContactLogs(List<ContactLogResponse>? contactLogs, string? searchText);
    }
}
