using ContactsManagement.Core.DTO.CompaniesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.CompaniesManagement
{
    /// <summary>
    /// Defines a service for adding a company to the system.
    /// </summary>
    public interface ICompanyAdderService
    {
        /// <summary>
        /// Adds a new company to the system.
        /// </summary>
        /// <param name="companyAddRequest">The request containing the data for the company to add.</param>
        /// <param name="userId">The ID of the user who owns the data.</param>
        ///  /// <returns>A <see cref="CompanyResponse"/> object containing the details of the added company.</returns>
        public Task<CompanyResponse> AddCompany(CompanyAddRequest companyAddRequest, Guid userId);
    }
}
