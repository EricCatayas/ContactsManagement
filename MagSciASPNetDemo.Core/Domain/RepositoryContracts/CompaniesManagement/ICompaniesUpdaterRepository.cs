using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.RepositoryContracts.CompaniesManagement
{
    /// <summary>
    /// Interface for a repository that updates information of a company.
    /// </summary>
    public interface ICompaniesUpdaterRepository
    {
        /// <summary>
        /// Updates the information of a company in the database.
        /// </summary> The company object containing the updated information.
        /// <param name="company"></param>
        /// <returns>The <see cref="Company"/> object that was updated from the database.</returns>
        Task<Company> UpdateCompany(Company company);
    }
}
