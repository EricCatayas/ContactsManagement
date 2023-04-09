using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.CompaniesManagement;
using ContactsManagement.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Infrastructure.Repositories.CompaniesManagement
{
    public class CompaniesGetterRepository : ICompaniesGetterRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<CompaniesGetterRepository> _logger;

        public CompaniesGetterRepository(ApplicationDbContext db, ILogger<CompaniesGetterRepository> logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task<List<Company>?> GetAllCompanies(Guid userId)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(CompaniesAdderRepository), nameof(GetAllCompanies));
            return await _db.Companies.Where(temp => temp.UserId == userId).Include("Employees").ToListAsync();
        }
        public async Task<Company?> GetCompanyById(int companyID, Guid userId)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(CompaniesAdderRepository), nameof(GetCompanyById));
            return await _db.Companies.Include("Employees").FirstOrDefaultAsync(temp => temp.CompanyId == companyID && temp.UserId == userId);
        }
    }
}
