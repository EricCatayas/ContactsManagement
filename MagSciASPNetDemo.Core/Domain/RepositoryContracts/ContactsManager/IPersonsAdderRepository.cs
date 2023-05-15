using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager
{
    /// <summary>
    /// Defines a repository for adding person to the database.
    /// </summary>
    public interface IPersonsAdderRepository
    {
        /// <summary>
        /// Adds a new person to the database.
        /// </summary>
        /// <param name="person">The person object to add.</param>
        /// <returns>The <see cref="Person"/> object that was added to the database.</returns>
        Task<Person> AddPerson(Person person);
    }
}
