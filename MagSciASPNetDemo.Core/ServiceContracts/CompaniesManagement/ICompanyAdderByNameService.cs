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
    public interface ICompanyAdderByNameService
    {
        /// <summary>
        /// Adds a new company to the system
        /// </summary>
        /// <param name="companyName"></param>
        /// <returns>The companyId of the added company</returns>
        public Task<int> AddCompanyByName(string companyName);
    }
}
