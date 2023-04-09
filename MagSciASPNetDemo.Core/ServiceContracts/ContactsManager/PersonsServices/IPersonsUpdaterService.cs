using ContactsManagement.Core.DTO.ContactsManager;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices
{
    /// <summary>
    /// Defines a service for updating a person data from the system.
    /// </summary>
    public interface IPersonsUpdaterService
    {
        /// <summary>
        /// Updates a person from the system.
        /// </summary>
        /// <param name="personUpdateRequest">The request containing the data for the person to be updated.</param>
        /// <returns>A <see cref="PersonResponse"/> object containing the details of the updated person.</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);
    }
}
