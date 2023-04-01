using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Infrastructure.DbContexts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Infrastructure.Repositories.ContactsManager.Persons
{
    public class PersonsDeleterRepository : IPersonsDeleterRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<PersonsDeleterRepository> _logger;

        public PersonsDeleterRepository(ApplicationDbContext db, ILogger<PersonsDeleterRepository> logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task<bool> DeletePerson(Person person)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(PersonsDeleterRepository), nameof(DeletePerson));
            _db.Persons.RemoveRange(person);
            int rowsDeleted = await _db.SaveChangesAsync();

            return rowsDeleted > 0;
        }

        public async Task<bool> DeletePersonById(Guid personID)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(PersonsDeleterRepository), nameof(DeletePersonById));
            _db.Persons.RemoveRange(_db.Persons.Where(temp => temp.Id == personID));
            int rowsDeleted = await _db.SaveChangesAsync();

            return rowsDeleted > 0;
        }
    }
}
