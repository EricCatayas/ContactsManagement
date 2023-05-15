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
        /// <returns><see langword="true"/> if contact tag with corresponding ID is deleted; otherwise, <see langword="false"/>.</returns>
        public Task<bool> DeleteContactTag(int contactTagId);
    }
}
