using ContactsManagement.Core.DTO.EventsManager;
using ContactsManagement.Core.Enums.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.EventsManager
{
    /// <summary>
    /// Defines a service for deleting a Event from the system.
    /// </summary>
    public interface IEventsGetterService
    {
        /// <summary>
        /// Retrieves list of Event from the system.
        /// </summary>
        /// <returns>A list of Event object with the corresponding UserId</returns>
        Task<List<EventResponse>?> GetEvents();
        /// <summary>
        /// Retrieves event from the system.
        /// </summary>
        /// <param name="eventId">The request Id of the Event to be retrived.</param>
        /// <returns>The Event object with the corresponding Id and UserId</returns>
        Task<EventResponse?> GetEventById(int eventId);
        /// <summary>
        /// Filters a list of events based on a search text.
        /// </summary>
        /// <param name="statusType">The status of the Event to filter by.</param>
        /// <returns>A filtered list of events.</returns>
        Task<List<EventResponse>>? GetFilteredEvents(StatusType statusType);
    }
}
