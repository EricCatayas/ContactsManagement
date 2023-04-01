using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Infrastructure.Repositories.ContactsManager.Persons
{
    public class PersonsGetterRepository : IPersonsGetterRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<PersonsGetterRepository> _logger;

        public PersonsGetterRepository(ApplicationDbContext db, ILogger<PersonsGetterRepository> logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task<List<Person>> GetAllPersons()
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(PersonsGetterRepository), nameof(GetAllPersons));
            return await _db.Persons.Include("Country").Include("Company").Include("Tag").Include("ContactGroups")
                .ToListAsync();
        }

        public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(PersonsGetterRepository), nameof(GetFilteredPersons));
            return await _db.Persons.Include("Country").Include("Company").Include("Tag").Include("ContactGroups")
             .Where(predicate)
             .ToListAsync();
        }

        public async Task<Person?> GetPersonById(Guid personID)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(PersonsGetterRepository), nameof(GetPersonById));
            return await _db.Persons.Include("Country").Include("Company").Include("Tag").Include("ContactGroups").Include("ContactLogs")
             .FirstOrDefaultAsync(temp => temp.Id == personID);
        }

        public async Task<List<Person>?> GetPersonsById(List<Guid>? personIDs)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(PersonsGetterRepository), nameof(GetPersonsById));
            if (personIDs == null) return null;
            return await _db.Persons.Include("Country").Include("Company").Include("Tag").Include("ContactGroups")
                .Where(person => personIDs.Contains(person.Id)).ToListAsync();
        }
    }
}
