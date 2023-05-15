using ContactsManagement.Core.DTO.CompaniesManagement;
using ContactsManagement.Core.DTO.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.CompaniesManagement
{
    /// <summary>
    /// Defines a service for retrieving company data from the system.
    /// </summary>
    public interface ICompanyGetterService
    {
        /// <summary>
        /// Retrieves company from the system.
        /// </summary>
        /// <param name="companyID">The request id of the company to be deleted.</param>
        /// <returns>The company object with the corresponding ID and UserID</returns>
        public Task<CompanyResponse?> GetCompanyById(int companyID);
        /// <summary>
        /// Retrieves list of company from the system.
        /// </summary>
        /// <returns>A list of company object with the corresponding UserID</returns>
        public Task<List<CompanyResponse>?> GetAllCompanies();
    }
}
