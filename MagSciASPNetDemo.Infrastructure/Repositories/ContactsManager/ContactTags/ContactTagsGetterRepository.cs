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
    public class ContactTagsGetterRepository : IContactTagsGetterRepository
    {
        private readonly ApplicationDbContext _db;

        public ContactTagsGetterRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<List<ContactTag>?> GetAllContactTags(Guid userId)
        {
            return await _db.ContactTags.Where(tag => tag.UserId == userId).ToListAsync();
        }
    }
}
