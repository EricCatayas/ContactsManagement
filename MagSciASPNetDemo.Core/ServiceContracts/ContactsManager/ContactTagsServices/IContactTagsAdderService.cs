using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.ContactTagsServices
{
    /// <summary>
    /// Defines a service for adding a contact tag to the system.
    /// </summary>
    public interface IContactTagsAdderService
    {
        /// <summary>
        /// Adds a new contact tag to the system.
        /// </summary>
        /// <param name="contactTagAddRequest">The request containing the data for the contact tag to add.</param>
        ///  /// <returns>A <see cref="ContactTagDTO"/> object containing the details of the added contact tag.</returns>
        Task<ContactTagDTO> AddContactTag(ContactTagAddRequest contactTagAddRequest);
    }
}
