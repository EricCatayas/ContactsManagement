using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.AccountManager
{
    /// <summary>
    /// Retrieves the unique identifier of the demo user account.
    /// </summary>
    public interface IDemoUserService
    {
        /// <summary>
        /// Gets the unique identifier of the demo user account.
        /// </summary>
        /// <returns>The unique identifier of the demo user account.</returns>
        Guid GetDemoUserId();
    }
}
