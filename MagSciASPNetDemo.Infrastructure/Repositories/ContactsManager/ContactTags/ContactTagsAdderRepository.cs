using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Infrastructure.DbContexts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Infrastructure.Repositories.ContactsManager.ContactTags
{
    public class ContactTagsAdderRepository : IContactTagsAdderRepository
    {
        private readonly ApplicationDbContext _db;

        public ContactTagsAdderRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<ContactTag> AddContactTag(ContactTag contactTag)
        {
            _db.ContactTags.Add(contactTag);  // database does not allow explicit creation of Id
            await _db.SaveChangesAsync();

            return contactTag;
        }
    }
}
