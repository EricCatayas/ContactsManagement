using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Infrastructure.Repositories.ContactsManager.ContactLogs
{
    public class ContactLogsAdderRepository : IContactLogsAdderRepository        
    {
        private readonly ApplicationDbContext _db;

        public ContactLogsAdderRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<ContactLog> AddContactLog(ContactLog contactLog)
        {
            _db.ContactLogs.Add(contactLog);
            await _db.SaveChangesAsync();
            return contactLog;
        }
    }
}
