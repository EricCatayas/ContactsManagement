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
        public async Task<bool> DeleteContactLog(ContactLog contactLog)
        {
            _db.ContactLogs.RemoveRange(contactLog);
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
