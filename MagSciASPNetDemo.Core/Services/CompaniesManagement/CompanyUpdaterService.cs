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
    public class CompanyUpdaterService : ICompanyUpdaterService
    {
        private readonly ICompaniesUpdaterRepository _companiesUpdaterRepository;
        private readonly IPersonsGetterRepository _personsGetterRepository;
        private readonly ISignedInUserService _signedInUserService;

        public CompanyUpdaterService(ICompaniesUpdaterRepository companiesUpdaterRepository, IPersonsGetterRepository personsGetterRepository, ISignedInUserService signedInUserService)
        {
            _companiesUpdaterRepository = companiesUpdaterRepository;
            _personsGetterRepository = personsGetterRepository;
            _signedInUserService = signedInUserService;
        }
        public async Task<CompanyResponse> UpdateCompany(CompanyUpdateRequest companyUpdateRequest)
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();

            Company company = companyUpdateRequest.ToCompany();
            company.UserId = userId;
            company.Employees = await _personsGetterRepository.GetPersonsById(companyUpdateRequest.Employees);
            company = await _companiesUpdaterRepository.UpdateCompany(company);

            return company.ToCompanyResponse();
        }
    }
}
