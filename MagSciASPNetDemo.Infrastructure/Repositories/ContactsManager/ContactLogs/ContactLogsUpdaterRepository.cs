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
    public class ContactLogsUpdaterRepository : IContactLogsUpdaterRepository
    {
        private readonly ApplicationDbContext _db;

        public ContactLogsUpdaterRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<ContactLog> UpdateContactLog(ContactLog contactLog)
        {
            ContactLog? matchingLog = await _db.ContactLogs.Include(log => log.PersonLog).FirstOrDefaultAsync(log => log.LogId== contactLog.LogId);
            if (matchingLog == null)
            {
                return contactLog;
            }
            matchingLog.LogTitle = contactLog.LogTitle;
            matchingLog.Note = contactLog.Note;
            matchingLog.Type = contactLog.Type;
            await _db.SaveChangesAsync();
            return matchingLog;
        }
    }
}
