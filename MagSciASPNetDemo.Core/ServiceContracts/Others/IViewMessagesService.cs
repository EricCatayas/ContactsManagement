using ContactsManagement.Core.Enums.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.Others
{
    /// <summary>
    /// Defines a service for managing status messages from the controllers to the view.
    /// </summary>
    public interface IViewMessagesService
    {
        void AddMessage(string message, RequestStatusType statusType);
        bool ContainsMessage(RequestStatusType statusType);
        List<string>? GetMessages(RequestStatusType statusType);
    }
}
