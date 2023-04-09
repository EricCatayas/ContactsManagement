using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.ContactLogsServices
{
    /// <summary>
    /// Defines a service for deleting a contact log from the system.
    /// </summary>
    public interface IContactLogsDeleterService
    {
        /// <summary>
        /// Deletes a contact log from the system.
        /// </summary>
        /// <param name="contactLogID">The request id of the contact log to be deleted.</param>
        /// <param name="userId">The ID of the user who owns the data.</param>
        /// <returns>True if contact log with corresponding ID is deleted, otherwise false.</returns>
        public Task<bool> DeleteContactLog(int contactLogId, Guid userId);
    }
}
