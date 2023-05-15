using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager
{
    /// <summary>
    /// Defines a repository for retrieving persons data from the database.
    /// </summary>
    public interface IPersonsGetterRepository
    {
        /// <summary>
        /// Retrieves all Person entities from the database.
        /// </summary>
        /// <param name="userId">The ID of the user associated with the Persons to be retrieved.</param>
        /// <returns>A list of Person entities associated with the user ID.</returns>
        Task<List<Person>> GetAllPersons(Guid userId);
        /// <summary>
        /// Retrieves the Person entity with the corresponding ID.
        /// </summary>
        /// <param name="personID">The ID of the Person to be retrieved.</param>
        /// <returns>The Person entity with the corresponding ID or null if not found.</returns>
        Task<Person?> GetPersonById(Guid personID);
        /// <summary>
        /// Retrieves a list of Person entities with the corresponding list of IDs.
        /// </summary>
        /// <param name="personIDs">The list of IDs of Persons to be retrieved.</param>
        /// <returns>A list of Person entities with the corresponding list of IDs or null if the input list is null.</returns>
        Task<List<Person>?> GetPersonsById(List<Guid>? personIDs);
        /// <summary>
        /// Retrieves a filtered list of Person entities with the corresponding user ID and filter predicate.
        /// </summary>
        /// <param name="predicate">The filter predicate to be applied.</param>
        /// <param name="userId">The ID of the user associated with the Persons to be retrieved.</param>
        /// <returns>A list of Person entities associated with the user ID and filtered by the filter predicate.</returns>
        Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate, Guid userId);
    }
}
