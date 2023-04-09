namespace ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices
{
    /// <summary>
    /// Defines a service for deleting a person from the system.
    /// </summary>
    public interface IPersonsDeleterService
    {
        /// <summary>
        /// Deletes a person from the system.
        /// </summary>
        /// <param name="personId">The request id of the person to be deleted.</param>
        /// <returns>True if person with corresponding ID is deleted, otherwise false.</returns>
        Task<bool> DeletePerson(Guid personId);
    }
}
