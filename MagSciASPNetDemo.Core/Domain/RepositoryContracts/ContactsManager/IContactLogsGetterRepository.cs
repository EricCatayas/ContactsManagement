using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager
{
    public interface IContactLogsGetterRepository
    {
        Task<ContactLog?> GetContactLog(int contactLogId, Guid userId);
        Task<List<ContactLog>?> GetContactLogs(Guid userId);
        Task<List<ContactLog>?> GetContactLogsFromPerson(Guid personId);
        
        // Task<List<ContactLog>> GetFilteredContactLogs(List<ContactLog> contactLogs ,Expression<Func<ContactLog, bool>> predicate);
    }
}
