using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager
{
    /// <summary>
    /// Interface for a repository that updates information of a person.
    /// </summary>
    public interface IPersonsUpdaterRepository
    {
        /// <summary>
        /// Updates the information of a person in the database.
        /// </summary> The person object containing the updated information.
        /// <param name="person"></param>
        /// <returns>The <see cref="Person"/> object that was updated from the database.</returns>
        Task<Person> UpdatePerson(Person person);
    }
}
