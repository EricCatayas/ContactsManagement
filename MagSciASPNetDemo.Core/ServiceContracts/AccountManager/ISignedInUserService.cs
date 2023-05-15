using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.AccountManager
{
    /// <summary>
    /// Defines a service for retrieving the current SignedIn User
    /// </summary>
    public interface ISignedInUserService
    {
        /// <summary>
        /// Retrieves UserId of current signed in user.
        /// </summary>
        /// <returns>UserId of the current signed in account.</returns>
        Guid? GetSignedInUserId();
    }
}
