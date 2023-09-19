using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager
{
    /// <summary>
    /// Defines a repository for adding contact group to the database.
    /// </summary>
    public interface IContactGroupsAdderRepository
    {
        /// <summary>
        /// Adds a new contact group to the database.
        /// </summary>
        /// <param name="contactGroup">The contact group object to add.</param>
        /// <returns>The <see cref="ContactGroup"/> object that was added to the database.</returns>
        Task<ContactGroup> AddContactGroup(ContactGroup contactGroup);
    }
}
