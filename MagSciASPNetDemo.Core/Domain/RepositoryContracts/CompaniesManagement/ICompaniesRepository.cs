using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.RepositoryContracts.CompaniesManagement
{
    public interface ICompaniesRepository
    {
        Task<Company> AddCompany(Company company);
        /// <summary>
        /// Suited for PersonsService.
        /// Principle of least astonishment: return the entity that is most likely to be expected by the calling code
        /// </summary>
        /// <param name="person"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        Task<Company> AddCompanyEmployee(Guid personId, Company company);
        /// <summary>
        /// Suited for CompaniesService
        /// </summary>
        /// <param name="persons"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        Task<Company> AddCompanyWithEmployees(Company company, List<Person>? persons);
        Task<bool> RemoveCompanyEmployee(Guid personId, Company company);
        /// <summary>
        /// Suited for CompaniesService
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        Task<Company> RemoveCompanyEmployees(Company company, List<Person> persons);
        Task<List<Company>?> GetAllCompanies();
        Task<List<Person>?> GetCompanyEmployees(int companyID);
        Task<Company?> GetCompanyById(int companyID);
        Task<Company> UpdateCompany(Company company);
        /// <summary>
        /// Removes the company and the associated references of the company object
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        Task<bool> DeleteCompany(Company company);
    }
}
