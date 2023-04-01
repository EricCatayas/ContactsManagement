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
    public class CompaniesRepository : ICompaniesRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<CompaniesRepository> _logger;
        public CompaniesRepository(ApplicationDbContext db, ILogger<CompaniesRepository> logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task<Company> AddCompany(Company Company)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(CompaniesRepository), nameof(AddCompany));

            Company.CompanyId = _db.Companies.Count() + 1;
            _db.Companies.Add(Company);
            await _db.SaveChangesAsync();

            return Company;
        }
        // Switched to Guid
        public async Task<Company> AddCompanyEmployee(Guid personId, Company? company)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(CompaniesRepository), nameof(AddCompanyEmployee));
            Person? person = _db.Persons.Include(p => p.Company).FirstOrDefault();
            try
            {
                if (company != null && person != null)
                {
                    company.Employees ??= new List<Person>();
                    company.Employees.Add(person);

                    /*Essentially, '_db.Entry(company).State = EntityState.Modified;' is a way of telling Entity Framework that the 'company' object has been modified and needs to be updated in the database when you call SaveChanges().*/
                    // _db.Entry(company).State = EntityState.Modified;
                    // _db.Entry(person).State = EntityState.Modified;
                }
                else
                {
                    throw new Exception();
                }

                await _db.SaveChangesAsync();

                return company;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding company employee; undoing transaction", ex);
            }
        }

        public async Task<Company> AddCompanyWithEmployees(Company company, List<Person>? persons)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(CompaniesRepository), nameof(AddCompanyEmployee));
            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                if (persons != null)
                {
                    foreach (Person person in persons)
                    {
                        company.Employees ??= new List<Person>();
                        company.Employees.Add(person);
                        // It's important to mark the entities as modified so that Entity Framework can detect the changes and save them to the database. In your code, you should call _db.Entry(company).State = EntityState.Modified; and _db.Entry(person).State = EntityState.Modified; to ensure that any changes made to the Company and Person entities are saved to the database.
                        // This could result in data inconsistencies, as the company object in your code won't reflect the state of the Company entity in the database.                      
                        person.Company = company;
                        // _db.Entry(person).State = EntityState.Modified;
                    }
                }
                _db.Companies.Add(company);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return company;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error adding company employees; undoing transaction", ex);
            }
        }

        public async Task<bool> DeleteCompany(Company company)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(CompaniesRepository), nameof(DeleteCompany));
            try
            {
                // If you remove a Company entity from the database that is referenced by one or more Person entities, and then call SaveChanges()
                // on the database context, Entity Framework will automatically remove the reference to the deleted Company entity from the related Person entities.
                /* if(company.Employees != null)
                 {
                     foreach (Person employee in company.Employees)
                     {
                         employee.Company = null;
                         _db.Entry(employee).State = EntityState.Modified;
                     }
                 }*/

                _db.Companies.RemoveRange(company);
                int rowsDeleted = await _db.SaveChangesAsync();

                return rowsDeleted > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing company and employees; undoing transaction", ex);
            }
        }

        public async Task<List<Company>?> GetAllCompanies()
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(CompaniesRepository), nameof(GetAllCompanies));
            return await _db.Companies.Include("Employees").ToListAsync();
        }

        public async Task<List<Person>?> GetCompanyEmployees(int companyID)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(CompaniesRepository), nameof(GetCompanyEmployees));
            Company? company = await _db.Companies.Include("Employees").FirstOrDefaultAsync(company => company.CompanyId == companyID);

            return company?.Employees?.ToList();
        }
        public async Task<Company?> GetCompanyById(int CompanyId)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(CompaniesRepository), nameof(GetCompanyById));
            return await _db.Companies.Include("Employees").FirstOrDefaultAsync(temp => temp.CompanyId == CompanyId);
        }

        public async Task<Company> UpdateCompany(Company Company)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(CompaniesRepository), nameof(UpdateCompany));
            Company? company = await _db.Companies.Include(c => c.Employees).FirstOrDefaultAsync(temp => temp.CompanyId == Company.CompanyId);

            if (company == null)
                return Company;
            company.CompanyName = Company.CompanyName;
            company.CompanyDescription = Company.CompanyDescription;
            company.Address1 = Company.Address1;
            company.Address2 = Company.Address2;
            company.ContactEmail = Company.ContactEmail;
            company.ContactNumber1 = Company.ContactNumber1;
            company.ContactNumber2 = Company.ContactNumber2;
            company.WebUrl = Company.WebUrl;
            company.Employees = Company.Employees;

            int countUpdated = await _db.SaveChangesAsync();
            return company;
        }
        public async Task<bool> RemoveCompanyEmployee(Guid personId, Company company)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(CompaniesRepository), nameof(RemoveCompanyEmployee));
            try
            {
                if (company.Employees != null)
                {
                    Person? person = company.Employees.FirstOrDefault(person => person.Id == personId);
                    company.Employees.Remove(person);
                }
                else
                {
                    return false;
                }
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing company employee; undoing transaction", ex);
            }
        }
        public async Task<Company> RemoveCompanyEmployees(Company company, List<Person> persons)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(CompaniesRepository), nameof(RemoveCompanyEmployees));
            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                foreach (Person person in persons)
                {
                    company.Employees.Remove(person);
                    _db.Entry(company).State = EntityState.Modified;

                    person.Company = null;
                    _db.Entry(person).State = EntityState.Modified;
                }
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return company;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error adding person with contact details; undoing transaction", ex);
            }

        }
    }
}
