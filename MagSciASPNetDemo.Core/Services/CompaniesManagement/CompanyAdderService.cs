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
    public class CompanyAdderService : ICompanyAdderService
    {
        private readonly ICompaniesAdderRepository _companiesAdderRepository;
        private readonly IPersonsGetterRepository _personsGetterRepository;

        public CompanyAdderService(ICompaniesAdderRepository companiesAdderRepository, IPersonsGetterRepository personsGetterRepository)
        {
            _companiesAdderRepository = companiesAdderRepository;
            _personsGetterRepository = personsGetterRepository;
        }
        public async Task<CompanyResponse> AddCompany(CompanyAddRequest companyAddRequest, Guid userId)
        {
            Company company = companyAddRequest.ToCompany();
            company.UserId = userId;

            company.Employees = await _personsGetterRepository.GetPersonsById(companyAddRequest.Employees);
            company = await _companiesAdderRepository.AddCompany(company);
            return company.ToCompanyResponse();
        }
    }
}
