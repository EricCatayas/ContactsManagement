using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.ContactGroupsServices
{
    /// <summary>
    /// Defines a service for adding a contact group to the system.
    /// </summary>
    public interface IContactGroupsAdderService
    {
        /// <summary>
        /// Adds a new contact group to the system.
        /// </summary>
        /// <param name="contactGroupAddRequest">The request containing the data for the contact group to add.</param>
        /// <param name="userId">The ID of the user who owns the data.</param>
        ///  /// <returns>A <see cref="ContactGroupResponse"/> object containing the details of the added contact group.</returns>
        Task<ContactGroupResponse> AddContactGroup(ContactGroupAddRequest contactGroupAddRequest, Guid userId);
    }
}
