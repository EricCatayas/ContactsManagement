using ContactsManagement.Core.DTO.CompaniesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.CompaniesManagement
{
    /// <summary>
    /// Defines a service for updating a company to the system.
    /// </summary>
    public interface ICompanyUpdaterService
    {
        /// <summary>
        /// Updates a company from the system.
        /// </summary>
        /// <param name="companyUpdateRequest">The request containing the data for the company to be updated.</param>
        /// <param name="userId">The ID of the user who owns the data.</param>
        ///  /// <returns>A <see cref="CompanyResponse"/> object containing the details of the updated company.</returns>
        public Task<CompanyResponse> UpdateCompany(CompanyUpdateRequest companyUpdateRequest);
    }
}
