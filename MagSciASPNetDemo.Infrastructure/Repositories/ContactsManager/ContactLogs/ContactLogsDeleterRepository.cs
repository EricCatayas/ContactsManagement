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
    public class ContactLogsDeleterRepository : IContactLogsDeleterRepository
    {
        private readonly ApplicationDbContext _db;

        public ContactLogsDeleterRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> DeleteContactLog(int contactLogId)
        {
            ContactLog? contactLog = await _db.ContactLogs.FirstOrDefaultAsync(log => log.LogId == contactLogId);

            _db.ContactLogs.Remove(contactLog);
            return await _db.SaveChangesAsync() > 0;
        }

        public  async Task<bool> DeleteContactLogsFromPerson(Person? person)
        {
            if (person == null) 
                return false;
            List<ContactLog>? contactLogs = person.ContactLogs.ToList();
            if (contactLogs == null) 
                return false;

            _db.ContactLogs.RemoveRange(contactLogs);
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
