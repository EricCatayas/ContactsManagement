namespace ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices
{
    public interface IPersonsDeleterService
    {
        /// <summary>
        /// Deletes the person with the specified Id
        /// </summary>
        /// <param name="personId">The person Id to search and delete</param>
        /// <returns>return True if found and deleted, otherwise false</returns>
        Task<bool> DeletePerson(Guid personId);
    }
}
