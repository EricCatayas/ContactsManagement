using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Infrastructure.Repositories.ContactsManager.ContactLogs
{
    public class ContactLogsGetterRepository : IContactLogsGetterRepository
    {
        private readonly ApplicationDbContext _db;

        public ContactLogsGetterRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<ContactLog?> GetContactLog(int contactLogId, Guid userId)
        {
            ContactLog? contactLog = await _db.ContactLogs.Include(log => log.PersonLog).FirstOrDefaultAsync(log => log.LogId == contactLogId && log.UserId == userId);
            return contactLog;
        }

        public async Task<List<ContactLog>?> GetContactLogs(Guid userId)
        {
            return await _db.ContactLogs.Where(log => log.UserId == userId).Include(log => log.PersonLog).ToListAsync();
        }

        public async Task<List<ContactLog>?> GetContactLogsFromPerson(Guid personId)
        {
            //Person? person = await _db.Persons.Include(p => p.ContactLogs).FirstOrDefaultAsync();
            List<ContactLog>? contactLogs = await _db.ContactLogs.Include(log => log.PersonLog).Where(log => log.PersonId == personId).ToListAsync();
            
            return contactLogs?.ToList();
        }
    }
}
