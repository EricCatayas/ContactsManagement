using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Infrastructure.Repositories.ContactsManager.ContactGroups
{
    public class ContactGroupsDeleterRepository : IContactGroupsDeleterRepository
    {
        private readonly ApplicationDbContext _db;

        public ContactGroupsDeleterRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> DeleteContactGroup(int contactGroupId, Guid userId)
        {
            ContactGroup? contactGroup = await _db.ContactGroups.FirstOrDefaultAsync(temp => temp.GroupId == contactGroupId && temp.UserId == userId);

            if (contactGroup == null) return false;
            _db.ContactGroups.RemoveRange(contactGroup);
            int rowsDeleted = await _db.SaveChangesAsync();

            return rowsDeleted > 0;
        }
    }
}
