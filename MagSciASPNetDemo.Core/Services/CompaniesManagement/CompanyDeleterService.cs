using ContactsManagement.Core.Domain.RepositoryContracts.CompaniesManagement;
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
    public class CompanyDeleterService : ICompanyDeleterService
    {
        private readonly ICompaniesDeleterRepository _companiesDeleterRepository;
        private readonly ISignedInUserService _signedInUserService;

        public CompanyDeleterService(ICompaniesDeleterRepository companiesDeleterRepository, ISignedInUserService signedInUserService) 
        {
            _companiesDeleterRepository = companiesDeleterRepository;
            _signedInUserService = signedInUserService;
        }
        public async Task<bool> DeleteCompany(int companyID)
        {
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();

            return await _companiesDeleterRepository.DeleteCompany(companyID, (Guid)userId);
        }
    }
}
