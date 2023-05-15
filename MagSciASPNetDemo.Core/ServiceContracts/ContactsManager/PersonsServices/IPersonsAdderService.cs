using ContactsManagement.Core.DTO.ContactsManager;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices
{
    /// <summary>
    /// Defines a service for adding a person to the system.
    /// </summary>
    public interface IPersonsAdderService
    {
        /// <summary>
        /// Adds a new person to the system.
        /// </summary>
        /// <param name="personAddRequest">The request containing the data for the person to add.</param>
        /// <param name="userId">The ID of the user who owns the data.</param>
        ///  /// <returns>A <see cref="PersonResponse"/> object containing the details of the added person.</returns>
        Task<PersonResponse?> AddPerson(PersonAddRequest? personAddRequest);
    }
}
