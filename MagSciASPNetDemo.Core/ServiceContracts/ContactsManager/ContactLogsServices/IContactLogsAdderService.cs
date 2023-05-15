using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.ContactLogsServices
{
    /// <summary>
    /// Defines a service for adding a contact log to the system.
    /// </summary>
    public interface IContactLogsAdderService
    {
        /// <summary>
        /// Adds a new contact log to the system.
        /// </summary>
        /// <param name="contactlogAddRequest">The request containing the data for the contact log to add.</param>
        ///  /// <returns>A <see cref="ContactLogResponse"/> object containing the details of the added contact log.</returns>
        public Task<ContactLogResponse> AddContactLog(ContactLogAddRequest contactLogAddRequest);
    }
}
