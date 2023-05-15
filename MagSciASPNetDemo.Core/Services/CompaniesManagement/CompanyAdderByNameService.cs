using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.CompaniesManagement;
using ContactsManagement.Core.DTO.CompaniesManagement;
using ContactsManagement.Core.Exceptions;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.CompaniesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.CompaniesManagement
{
    public class CompanyAdderByNameService : ICompanyAdderByNameService
    {
        private readonly ICompaniesAdderRepository _companiesAdderRepository;
        private readonly ISignedInUserService _signedInUserService;

        public CompanyAdderByNameService(ICompaniesAdderRepository companiesAdderRepository, ISignedInUserService signedInUserService)
        {
            _companiesAdderRepository = companiesAdderRepository;
            _signedInUserService = signedInUserService;
        }
        public async Task<int> AddCompanyByName(string companyName)
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();
            Company companyToAdd = new()
            {
                CompanyName = companyName,
                UserId = userId
            };
            Company addedCompany = await _companiesAdderRepository.AddCompany(companyToAdd);
            return addedCompany.CompanyId;
        }
    }
}
