using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.ContactTagsServices
{
    /// <summary>
    /// Defines a service for seeding the contact tag data of a user.
    /// </summary>
    public interface IContactTagsSeederService
    {
        /// <summary>
        /// Adds a list of contact tag object to the data of the user.
        /// </summary>
        /// <param name="userId">The ID of the user who owns the data.</param>
        Task SeedUserContactTags(Guid userId);
    }
}
