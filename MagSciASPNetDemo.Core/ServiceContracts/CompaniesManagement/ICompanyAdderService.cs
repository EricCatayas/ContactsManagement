using ContactsManagement.Core.DTO.CompaniesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.CompaniesManagement
{
    public interface ICompanyAdderService
    {
        public Task<CompanyResponse> AddCompany(CompanyAddRequest companyAddRequest);
    }
}
