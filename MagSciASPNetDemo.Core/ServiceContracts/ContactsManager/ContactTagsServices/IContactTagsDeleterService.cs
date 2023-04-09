using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.ContactTagsServices
{
    /// <summary>
    /// Defines a service for deleting a contact tag from the system.
    /// </summary>
    public interface IContactTagsDeleterService
    {
        /// <summary>
        /// Deletes a contact tag from the system.
        /// </summary>
        /// <param name="contactTagId">The request id of the contact tag to be deleted.</param>
        /// <param name="userId">The ID of the user who owns the data.</param>
        /// <returns>True if contact tag with corresponding ID is deleted, otherwise false.</returns>
        public Task<bool> DeleteContactTag(int contactTagId, Guid userId);
    }
}
