using ContactsManagement.Core.DTO.EventsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.EventsManager
{
    /// <summary>
    /// Defines a service for adding a event to the system.
    /// </summary>
    public interface IEventsAdderService
    {
        /// <summary>
        /// Adds a new event to the system.
        /// </summary>
        /// <param name="eventAddRequest">The request containing the data for the event to add.</param>
        /// <param name="userId">The ID of the user who owns the data.</param>
        ///  /// <returns>A <see cref="EventResponse"/> object containing the details of the added event.</returns>
        Task<EventResponse> AddEvent(EventAddRequest eventAddRequest, Guid userId);
    }
}
