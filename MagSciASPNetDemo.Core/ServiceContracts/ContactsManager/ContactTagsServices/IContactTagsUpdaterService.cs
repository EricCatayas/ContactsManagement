using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.ContactTagsServices
{
    /// <summary>
    /// Defines a service for updating the contact tag from the system
    /// </summary>
    public interface IContactTagsUpdaterService
    {
        /// <summary>
        /// Removes a contact tag from a person.
        /// </summary>
        /// <param name="personId">The request ID of the person.</param>
        /// <returns>True if contact tag from person with corresponding ID is removed, otherwise false.</returns>
        Task<bool> RemoveContactTagFromPerson(Guid personId);
    }
}
