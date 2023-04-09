using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager
{
    public interface IPersonsGetterRepository
    {
        /// <summary>
        /// Returns all persons in the data store
        /// </summary>
        /// <returns>List of person objects from table</returns>
        Task<List<Person>> GetAllPersons(Guid userId);


        /// <summary>
        /// Returns a person object based on the given person id
        /// </summary>
        /// <param name="personID">PersonID (guid) to search</param>
        /// <returns>A person object or null</returns>
        Task<Person?> GetPersonById(Guid personID);
        /// <summary>
        /// Returns a list of person object based on the given list of person id
        /// </summary>
        /// <param name="personID">PersonID (guid) to search</param>
        /// <returns>A list of person object or null</returns>
        Task<List<Person>?> GetPersonsById(List<Guid>? personIDs);


        /// <summary>
        /// Returns all person objects based on the given expression
        /// </summary>
        /// <param name="predicate">LINQ expression to check</param>
        /// <returns>All matching persons with given condition</returns>
        Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate, Guid userId);
    }
}
