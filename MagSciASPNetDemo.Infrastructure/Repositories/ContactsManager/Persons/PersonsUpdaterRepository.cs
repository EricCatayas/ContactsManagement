using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Infrastructure.Repositories.ContactsManager.Persons
{
    public class PersonsUpdaterRepository : IPersonsUpdaterRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<PersonsUpdaterRepository> _logger;

        public PersonsUpdaterRepository(ApplicationDbContext db, ILogger<PersonsUpdaterRepository> logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task<Person> UpdatePerson(Person person)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(PersonsUpdaterRepository), nameof(UpdatePerson));
            Person? matchingPerson = await _db.Persons.Include("Country").Include("Company").Include("Tag").Include("ContactGroups").Include("ContactLogs").FirstOrDefaultAsync(temp => temp.Id == person.Id);

            if (matchingPerson == null)
                return person;

            matchingPerson.Name = person.Name;
            matchingPerson.Email = person.Email;
            matchingPerson.DateOfBirth = person.DateOfBirth;
            matchingPerson.Gender = person.Gender;
            // matchingPerson.Country = person.Country; <-- assuming country is invalid, throws exception
            matchingPerson.Address = person.Address;
            matchingPerson.CountryId = person.CountryId;
            matchingPerson.JobTitle = person.JobTitle;
            matchingPerson.ContactNumber1 = person.ContactNumber1;
            matchingPerson.ContactNumber2 = person.ContactNumber2;
            matchingPerson.CompanyId = person.CompanyId;
            matchingPerson.TagId = person.TagId;
            matchingPerson.ProfileBlobUrl = person.ProfileBlobUrl;

            matchingPerson.ContactGroups = person.ContactGroups;

            /* If any of the existing ICollection<T> entities associated with the matchingPerson entity are not present in the updated person.ICollection<T>, those entities will no longer be associated with the matchingPerson entity after the call to _db.SaveChangesAsync(), which will cause them to be deleted from the database due to the default behavior of EF Core when working with many-to-many relationships.*/

            /* Updation of ContactGroup ContactLog  is handled in corresponding repositories*/

            int countUpdated = await _db.SaveChangesAsync();

            return matchingPerson;
        }
    }
}
