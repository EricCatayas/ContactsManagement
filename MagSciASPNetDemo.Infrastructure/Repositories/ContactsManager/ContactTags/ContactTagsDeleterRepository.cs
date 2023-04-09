using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Infrastructure.Repositories.ContactsManager.ContactTags
{
    public class ContactTagsDeleterRepository : IContactTagsDeleterRepository
    {
        private readonly ApplicationDbContext _db;

        public ContactTagsDeleterRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> DeleteContactTagById(int contactTagId, Guid userId)
        {
            _db.ContactTags.RemoveRange(_db.ContactTags.Where(temp => temp.TagId == contactTagId && temp.UserId == userId));

            int rowsDeleted = await _db.SaveChangesAsync();

            return rowsDeleted > 0;
        }
    }
}
