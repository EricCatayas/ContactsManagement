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
        /// <param name="userId">The Id of the user who owns the data.</param>
        /// <returns>A list of Event object with the corresponding UserId</returns>
        Task<List<EventResponse>?> GetEvents(Guid userId);
        /// <summary>
        /// Retrieves event from the system.
        /// </summary>
        /// <param name="eventId">The request Id of the Event to be retrived.</param>
        /// <param name="userId">The Id of the user who owns the data.</param>
        /// <returns>The Event object with the corresponding Id and UserId</returns>
        Task<EventResponse?> GetEventById(int eventId, Guid userId);
        /// <summary>
        /// Filters a list of events based on a search text.
        /// </summary>
        /// <param name="events">list of Event to be filtered.</param>
        /// <param name="statusType">The search text to filter by.</param>
        /// <returns>A filtered list of events.</returns>
        List<EventResponse>? GetFilteredEvents(List<EventResponse>? events, StatusType statusType);
    }
}
