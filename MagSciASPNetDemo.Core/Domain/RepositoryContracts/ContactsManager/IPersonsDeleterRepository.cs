using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager
{
    /// <summary>
    /// Defines a repository for deleting person data from the database.
    /// </summary>
    public interface IPersonsDeleterRepository
    {
        /// <summary>
        /// Deletes a person from the database.
        /// </summary>
        /// <param name="personID">The ID of the person to delete.</param>
        /// <returns><see langword="true"/> if the person was deleted from the repository; otherwise, <see langword="false"/>.</returns>
        Task<bool> DeletePersonById(Guid personID);
        /// <summary>
        /// Deletes a person from the database.
        /// </summary>
        /// <param name="person">The person to delete.</param>
        /// <returns><see langword="true"/> if the person was deleted from the repository; otherwise, <see langword="false"/>.</returns>
        Task<bool> DeletePerson(Person person);
    }
}
