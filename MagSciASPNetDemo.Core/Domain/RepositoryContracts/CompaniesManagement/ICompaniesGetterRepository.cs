using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.RepositoryContracts.CompaniesManagement
{
    /// <summary>
    /// Defines a repository for retrieving companies data from the database.
    /// </summary>
    public interface ICompaniesGetterRepository
    {
        /// <summary>
        /// Retrieves all Company entities from the database.
        /// </summary>
        /// <param name="userId">The ID of the user associated with the Companies to be retrieved.</param>
        /// <returns>A list of <see cref="Company"/> entities.</returns>
        Task<List<Company>?> GetAllCompanies(Guid userId);
        /// <summary>
        ///  Retrieves a list of Company entities with the corresponding ID.
        /// </summary>
        /// <param name="companyID">The ID of the Company to be retrieved.</param>
        /// <param name="userId">The ID of the user associated with the Company to be retrieved.</param>
        /// <returns>The <see cref="Company"/> entity with the corresponding ID or null if not found.</returns>
        Task<Company?> GetCompanyById(int companyID, Guid userId);
    }
}
