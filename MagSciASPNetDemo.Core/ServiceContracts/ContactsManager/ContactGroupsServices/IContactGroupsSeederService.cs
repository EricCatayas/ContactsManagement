using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.ContactGroupsServices
{
    /// <summary>
    /// Defines a service for seeding the contact group data of a user.
    /// </summary>
    public interface IContactGroupsSeederService
    {
        /// <summary>
        /// Adds a list of contact group object to the user.
        /// </summary>
        /// <param name="userId">The ID of the user who owns the data.</param>
        Task SeedUserContactGroups(Guid userId);
    }
}
