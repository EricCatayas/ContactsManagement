using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.CompaniesManagement;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.CompaniesManagement;
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
    public class CompanyAdderService : ICompanyAdderService
    {
        private readonly ICompaniesAdderRepository _companiesAdderRepository;
        private readonly IPersonsGetterRepository _personsGetterRepository;
        private readonly ISignedInUserService _signedInUserService;

        public CompanyAdderService(ICompaniesAdderRepository companiesAdderRepository, IPersonsGetterRepository personsGetterRepository, ISignedInUserService signedInUserService)
        {
            _companiesAdderRepository = companiesAdderRepository;
            _personsGetterRepository = personsGetterRepository;
            _signedInUserService = signedInUserService;
        }
        public async Task<CompanyResponse> AddCompany(CompanyAddRequest companyAddRequest)
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();

            Company company = companyAddRequest.ToCompany();
            company.UserId = userId;

            company.Employees = await _personsGetterRepository.GetPersonsById(companyAddRequest.Employees);
            company = await _companiesAdderRepository.AddCompany(company);
            return company.ToCompanyResponse();
        }
    }
}
