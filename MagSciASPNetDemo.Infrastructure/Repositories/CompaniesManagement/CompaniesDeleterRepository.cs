using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.CompaniesManagement;
using ContactsManagement.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Infrastructure.Repositories.CompaniesManagement
{
    public class CompaniesDeleterRepository : ICompaniesDeleterRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<CompaniesDeleterRepository> _logger;

        public CompaniesDeleterRepository(ApplicationDbContext db, ILogger<CompaniesDeleterRepository> logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task<bool> DeleteCompany(int companyId, Guid userId)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(CompaniesDeleterRepository), nameof(DeleteCompany));

            Company? company = await _db.Companies.FirstOrDefaultAsync(temp => temp.CompanyId == companyId && temp.UserId == userId);
            if(company == null)
            {
                return false;
            }
            _db.Companies.RemoveRange(company);
            int rowsDeleted = await _db.SaveChangesAsync();

            return rowsDeleted > 0;
            }
    }
}
