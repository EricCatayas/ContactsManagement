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
    public class PersonsAdderRepository : IPersonsAdderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<PersonsAdderRepository> _logger;

        public PersonsAdderRepository(ApplicationDbContext db, ILogger<PersonsAdderRepository> logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task<Person> AddPerson(Person person)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(PersonsAdderRepository), nameof(AddPerson));
            _db.Persons.Add(person);
            await _db.SaveChangesAsync();

            return person;
        }
    }
}
