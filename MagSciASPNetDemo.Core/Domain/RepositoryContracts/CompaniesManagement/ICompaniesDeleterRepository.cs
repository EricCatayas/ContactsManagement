using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.RepositoryContracts.CompaniesManagement
{
    /// <summary>
    /// Defines a repository for deleting company data from the database.
    /// </summary>
    public interface ICompaniesDeleterRepository
    {
        /// <summary>
        /// Deletes a company from the database.
        /// </summary>
        /// <param name="companyId">The ID of the company to delete.</param>
        /// <param name="userId">The ID of the user who owns the company.</param>
        /// <returns><see langword="true"/> if the company was deleted from the database; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// This method deletes the company with the specified <paramref name="companyId"/> and <paramref name="userId"/> from the database.
        /// If the company does not exist in the database, the method returns <see langword="false"/> without modifying the database.
        /// If the company is successfully deleted from the database, the method returns <see langword="true"/>.
        /// </remarks>
        Task<bool> DeleteCompany(int companyId, Guid userId);
    }
}
