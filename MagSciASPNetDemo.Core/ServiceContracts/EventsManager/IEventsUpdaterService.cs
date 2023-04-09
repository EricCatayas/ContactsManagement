using ContactsManagement.Core.DTO.EventsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.EventsManager
{
    /// <summary>
    /// Defines a service for updating a Event data from the system.
    /// </summary>
    public interface IEventsUpdaterService
    {
        /// <summary>
        /// Updates an Event from the system.
        /// </summary>
        /// <param name="eventUpdateRequest">The request containing the data for the Event to be updated.</param>
        /// <returns>True if Event with corresponding ID is updated, otherwise false.</returns>
        Task<bool> UpdateEvent(EventUpdateRequest eventUpdateRequest);
        /// <summary>
        /// Removes an Event from a Event.
        /// </summary>
        /// <param name="eventId">The request ID of the Event.</param>
        /// <param name="userId">The ID of the user who owns the data.</param>
        /// <returns>True if Event.IsCompleted with corresponding ID is updated, otherwise false.</returns>
        Task<bool> UpdateEventCompletion(int  eventId, Guid userId);
    }
}
