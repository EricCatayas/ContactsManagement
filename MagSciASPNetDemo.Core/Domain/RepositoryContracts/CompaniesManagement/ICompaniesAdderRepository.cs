using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.RepositoryContracts.CompaniesManagement
{
    /// <summary>
    /// Defines a repository for adding companies to the database.
    /// </summary>
    public interface ICompaniesAdderRepository
    {
        /// <summary>
        /// Adds a new company to the database.
        /// </summary>
        /// <param name="company">The company object to add.</param>
        /// <returns>The <see cref="Company"/> object that was added to the database.</returns>
        Task<Company> AddCompany(Company company);
    }
}
