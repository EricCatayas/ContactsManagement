using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.Services.ContactsManager;
using ContactsManagement.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Infrastructure.Repositories.ContactsManager.ContactGroups
{
    public class ContactGroupsAdderRepository : IContactGroupsAdderRepository
    {
        private readonly ApplicationDbContext _db;

        public ContactGroupsAdderRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<ContactGroup> AddContactGroup(ContactGroup contactGroup, List<Guid>? persons)
        {
            _db.ContactGroups.Add(contactGroup);  // Id is configured to be auto generated
            _db.SaveChanges();

            if (persons != null)
            {
                List<Person>? personsList = await _db.Persons.Where(person => persons.Contains(person.Id)).ToListAsync();

                if (personsList != null)
                {
                    contactGroup.Persons = personsList;
                    foreach (Person person in personsList)
                    {
                        person.ContactGroups ??= new List<ContactGroup>();
                        person.ContactGroups.Add(contactGroup);
                    }
                    await _db.SaveChangesAsync();
                }

            }
            return contactGroup;
        }
    }
}
