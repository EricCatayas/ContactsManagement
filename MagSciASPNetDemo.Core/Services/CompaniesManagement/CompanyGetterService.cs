using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.CompaniesManagement;
using ContactsManagement.Core.DTO.CompaniesManagement;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.Exceptions;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.CompaniesManagement;
using ContactsManagement.Core.Services.AccountManager;
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
        private readonly ISignedInUserService _signedInUserService;

        public CompanyGetterService(ICompaniesGetterRepository companiesGetterRepository, ISignedInUserService signedInUserService)
        {
            _companiesGetterRepository = companiesGetterRepository;
            _signedInUserService = signedInUserService;
        }
        public async Task<List<CompanyResponse>?> GetAllCompanies()
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();
            List<Company>? companies = await _companiesGetterRepository.GetAllCompanies((Guid)userId);
            return companies?.Select(company => company.ToCompanyResponse()).ToList().OrderBy(company => company.CompanyName).ToList();
        }

        public async Task<CompanyResponse?> GetCompanyById(int companyID)
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();
            Company? company = await _companiesGetterRepository.GetCompanyById(companyID, (Guid)userId);
            return company?.ToCompanyResponse();
        }
    }
}
