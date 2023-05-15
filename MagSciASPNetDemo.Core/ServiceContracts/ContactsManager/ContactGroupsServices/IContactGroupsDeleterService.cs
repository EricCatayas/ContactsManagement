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
    /// Defines a service for deleting a contact group from the system.
    /// </summary>
    public interface IContactGroupsDeleterService
    {
        /// <summary>
        /// Deletes a contact group from the system.
        /// </summary>
        /// <param name="contactGroupId">The request id of the contact group to be deleted.</param>
        /// <returns><see langword="true"/> if contact group with corresponding ID is deleted; otherwise, <see langword="false"/>.</returns>
        Task<bool> DeleteContactGroup(int contactGroupId);
    }
}
