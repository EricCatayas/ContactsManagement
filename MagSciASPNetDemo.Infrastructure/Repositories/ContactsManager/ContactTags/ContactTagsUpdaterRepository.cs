using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Infrastructure.Repositories.ContactsManager.ContactTags
{
    public class ContactTagsUpdaterRepository : IContactTagsUpdaterRepository
    {
        private readonly ApplicationDbContext _db;

        public ContactTagsUpdaterRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> UpdatePersonContactTag(Person person)
        {
            Person? matchingPerson = await _db.Persons.FirstOrDefaultAsync(temp => temp.Id == person.Id);

            if (matchingPerson == null)
                return false;
            matchingPerson.TagId = person.TagId;

            int countUpdated = await _db.SaveChangesAsync();
            return countUpdated > 0;
        }
    }
}
