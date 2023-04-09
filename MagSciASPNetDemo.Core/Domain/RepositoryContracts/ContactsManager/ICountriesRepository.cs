using ContactsManagement.Core.Domain.Entities.ContactsManager;

namespace ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager
{
    /// <summary>
    /// Represents data access logic for managing Person entity
    /// </summary>
    public interface ICountriesRepository
    {
        /// <summary>
        /// Returns all countries in the data store
        /// </summary>
        /// <returns>All countries from the table</returns>
        Task<List<Country>> GetAllCountries();
    }
}