using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.CompaniesManagement;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.CompaniesManagement;
using ContactsManagement.Core.ServiceContracts.CompaniesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.CompaniesManagement
{
    public class CompanyUpdaterService : ICompanyUpdaterService
    {
        private readonly ICompaniesUpdaterRepository _companiesUpdaterRepository;
        private readonly IPersonsGetterRepository _personsGetterRepository;

        public CompanyUpdaterService(ICompaniesUpdaterRepository companiesUpdaterRepository, IPersonsGetterRepository personsGetterRepository)
        {
            _companiesUpdaterRepository = companiesUpdaterRepository;
            _personsGetterRepository = personsGetterRepository;
        }
        public async Task<CompanyResponse> UpdateCompany(CompanyUpdateRequest companyUpdateRequest)
        {
            Company company = companyUpdateRequest.ToCompany();
            company.Employees = await _personsGetterRepository.GetPersonsById(companyUpdateRequest.Employees);
            company = await _companiesUpdaterRepository.UpdateCompany(company);

            return company.ToCompanyResponse();
        }
    }
}
