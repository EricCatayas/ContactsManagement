using ContactsManagement.Core.DTO.CompaniesManagement;
using ContactsManagement.Core.DTO.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.CompaniesManagement
{
    public interface ICompanyGetterService
    {
        public Task<CompanyResponse?> GetCompanyById(int companyID);
        public Task<List<CompanyResponse>?> GetAllCompanies();
        public Task<List<PersonResponse>?> GetCompanyEmployees(int companyID);
    }
}
