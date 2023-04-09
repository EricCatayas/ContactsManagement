using ContactsManagement.Core.Domain.RepositoryContracts.CompaniesManagement;
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

        public CompanyDeleterService(ICompaniesDeleterRepository companiesDeleterRepository)
        {
            _companiesDeleterRepository = companiesDeleterRepository;
        }
        public async Task<bool> DeleteCompany(int companyID, Guid userId)
        {
            return await _companiesDeleterRepository.DeleteCompany(companyID, userId);
        }
    }
}
