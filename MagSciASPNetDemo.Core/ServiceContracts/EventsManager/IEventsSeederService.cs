using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.EventsManager
{
    /// <summary>
    /// Defines a service for seeding the Event data of a user.
    /// </summary>
    public interface IEventsSeederService
    {
        /// <summary>
        /// Adds a list of Event object to the data of the user.
        /// </summary>
        /// <param name="userId">The ID of the user who owns the data.</param>
        Task SeedUserEvents(Guid userId);
    }
}
