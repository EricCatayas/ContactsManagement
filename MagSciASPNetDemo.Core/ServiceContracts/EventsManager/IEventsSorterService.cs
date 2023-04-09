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
    /// Defines a service for sorting a list of Event data from the system.
    /// </summary>
    public interface IEventsSorterService
    {
        /// <summary>
        /// Returns the sorted list of Event in either ascending or descending order
        /// </summary>
        /// <param name="events">the list of Event to be sorted</param>
        /// <param name="sortBy">the field property of Event to be sorted by</param>
        /// <param name="sortOrder">Ascending or Descending</param>
        /// <returns>A list of Event in sorted order</returns>
        List<EventResponse>? GetSortedEvents(List<EventResponse>? events, string? sortBy, SortOrderOptions sortOrder);
    }
}
