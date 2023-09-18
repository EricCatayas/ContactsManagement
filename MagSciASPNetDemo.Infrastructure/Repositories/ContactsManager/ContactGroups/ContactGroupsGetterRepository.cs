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
    public class ContactGroupsGetterRepository : IContactGroupsGetterRepository
    {
        private readonly ApplicationDbContext _db;

        public ContactGroupsGetterRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<ContactGroup?> GetContactGroupById(int contactGroupId, Guid userId)
        {
            return await _db.ContactGroups.Include(p => p.Persons).FirstOrDefaultAsync(c => c.GroupId == contactGroupId && c.UserId == userId);
        }

        public async Task<List<ContactGroup>> GetContactGroups(Guid userId)
        {
            return await _db.ContactGroups.Where(c => c.UserId == userId).Include(p => p.Persons).ToListAsync();
        }

        public async Task<List<ContactGroup>> GetContactGroupsById(List<int>? contactGroupIds)
        {
            return await _db.ContactGroups.Where(c => contactGroupIds.Contains(c.GroupId)).Include(p => p.Persons).ToListAsync();
        }
    }
}
