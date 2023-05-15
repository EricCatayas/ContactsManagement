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
    public class CompaniesUpdaterRepository : ICompaniesUpdaterRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<CompaniesUpdaterRepository> _logger;

        public CompaniesUpdaterRepository(ApplicationDbContext db, ILogger< CompaniesUpdaterRepository > logger)
        {
                _db = db;
                _logger = logger;
        }
        public async Task<Company> UpdateCompany(Company company)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(CompaniesUpdaterRepository), nameof(UpdateCompany));
            Company? companyToUpdate = await _db.Companies.Include(c => c.Employees).FirstOrDefaultAsync(temp => temp.CompanyId == company.CompanyId && temp.UserId == company.UserId);

            if (companyToUpdate == null)
                return company;
            companyToUpdate.CompanyName = company.CompanyName;
            companyToUpdate.CompanyDescription = company.CompanyDescription;
            companyToUpdate.Industry = company.Industry;
            companyToUpdate.Address1 = company.Address1;
            companyToUpdate.Address2 = company.Address2;
            companyToUpdate.ContactEmail = company.ContactEmail;
            companyToUpdate.ContactNumber1 = company.ContactNumber1;
            companyToUpdate.ContactNumber2 = company.ContactNumber2;
            companyToUpdate.WebUrl = company.WebUrl;
            companyToUpdate.Employees = company.Employees;

            int countUpdated = await _db.SaveChangesAsync();
            return company;
        }
    }
}
