using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.CompaniesManagement;
using ContactsManagement.Core.DTO.CompaniesManagement;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.ServiceContracts.CompaniesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.CompaniesManagement
{
    public class CompanyGetterService : ICompanyGetterService
    {
        private readonly ICompaniesGetterRepository _companiesGetterRepository;
        public CompanyGetterService(ICompaniesGetterRepository companiesGetterRepository)
        {
            _companiesGetterRepository = companiesGetterRepository;
        }
        public async Task<List<CompanyResponse>?> GetAllCompanies(Guid userId)
        {
            List<Company>? companies = await _companiesGetterRepository.GetAllCompanies(userId);
            return companies?.Select(company => company.ToCompanyResponse()).ToList().OrderBy(company => company.CompanyName).ToList();
        }

        public async Task<CompanyResponse?> GetCompanyById(int companyID, Guid userId)
        {
            Company? company = await _companiesGetterRepository.GetCompanyById(companyID, userId);
            return company?.ToCompanyResponse();
        }
    }
}
