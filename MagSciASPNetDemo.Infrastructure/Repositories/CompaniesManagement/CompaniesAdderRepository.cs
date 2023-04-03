using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.CompaniesManagement;
using ContactsManagement.Infrastructure.DbContexts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Infrastructure.Repositories.CompaniesManagement
{
    public class CompaniesAdderRepository : ICompaniesAdderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<CompaniesAdderRepository> _logger;

        public CompaniesAdderRepository(ApplicationDbContext db, ILogger<CompaniesAdderRepository> logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task<Company> AddCompany(Company Company)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(CompaniesAdderRepository), nameof(AddCompany));

            _db.Companies.Add(Company);
            await _db.SaveChangesAsync();

            return Company;
        }
    }
}
