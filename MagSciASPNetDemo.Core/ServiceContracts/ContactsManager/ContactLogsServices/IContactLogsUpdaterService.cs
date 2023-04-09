using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.ContactLogsServices
{
    /// <summary>
    /// Defines a service for updating the contact log from the system
    /// </summary>
    public interface IContactLogsUpdaterService
    {
        /// <summary>
        /// Updates a contact log from the system.
        /// </summary>
        /// <param name="contactLogUpdateRequest">The request containing the data for the contact log to be updated.</param>
        ///  /// <returns>A <see cref="ContactLogResponse"/> object containing the details of the updated contact log.</returns>
        public Task<ContactLogResponse> UpdateContactLog(ContactLogUpdateRequest contactLogUpdateRequest);
    }
}
