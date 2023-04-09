using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.ContactTagsServices
{
    /// <summary>
    /// Defines a service for retrieving contact tag data from the system.
    /// </summary>
    public interface IContactTagsGetterService
    {
        /// <summary>
        /// Retrieves a list of contact tag from the system.
        /// </summary>
        /// <param name="userId">The ID of the user who owns the data.</param>
        /// <returns>A list of contact tag object with the corresponding UserID</returns>
        Task<List<ContactTagDTO>?> GetAllContactTags(Guid userId);
    }
}
