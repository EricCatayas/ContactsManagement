using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.ContactGroupsServices
{
    /// <summary>
    /// Defines a service for updating the contact group from the system
    /// </summary>
    public interface IContactGroupsUpdaterService
    {
        /// <summary>
        /// Updates a contact group from the system.
        /// </summary>
        /// <param name="contactGroupUpdateRequest">The request containing the data for the contact group to be updated.</param>
        /// <returns>A <see cref="ContactGroupResponse"/> object containing the details of the updated contact group.</returns>
        Task<ContactGroupResponse?> UpdateContactGroup(ContactGroupUpdateRequest contactGroupUpdateRequest);
    }
}
