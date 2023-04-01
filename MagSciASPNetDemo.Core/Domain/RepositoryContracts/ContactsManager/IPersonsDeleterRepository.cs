using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager
{
    public interface IPersonsDeleterRepository
    {
        /// <summary>
        /// Deletes a person object based on the person id
        /// </summary>
        /// <param name="personID">Person ID (guid) to search</param>
        /// <returns>Returns true, if the deletion is successful; otherwise false</returns>
        Task<bool> DeletePersonById(Guid personID);
        /// <summary>
        /// Deletes a person object based on the person
        /// </summary>
        /// <param name="personID">Person ID (guid) to search</param>
        /// <returns>Returns true, if the deletion is successful; otherwise false</returns>
        Task<bool> DeletePerson(Person person);
    }
}
