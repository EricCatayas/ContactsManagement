using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.EventsManager
{
    /// <summary>
    /// Defines a service for deleting a event from the system.
    /// </summary>
    public interface IEventsDeleterService
    {
        /// <summary>
        /// Deletes a event from the system.
        /// </summary>
        /// <param name="eventId">The request id of the event to be deleted.</param>
        /// <returns>True if event with corresponding ID is deleted, otherwise false.</returns>
        Task<bool> DeleteEvent(int eventId, Guid userId);
    }
}
