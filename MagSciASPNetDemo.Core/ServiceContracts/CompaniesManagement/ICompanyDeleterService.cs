using ContactsManagement.Core.DTO.CompaniesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.CompaniesManagement
{
    /// <summary>
    /// Defines a service for deleting a company from the system.
    /// </summary>
    public interface ICompanyDeleterService
    {
        /// <summary>
        /// Deletes a company from the system.
        /// </summary>
        /// <param name="companyID">The request id of the company to be deleted.</param>
        /// <param name="userId">The ID of the user who owns the data.</param>
        /// <returns>True if company with corresponding ID is deleted, otherwise false.</returns>
        public Task<bool> DeleteCompany(int companyID, Guid userId);
    }
}
