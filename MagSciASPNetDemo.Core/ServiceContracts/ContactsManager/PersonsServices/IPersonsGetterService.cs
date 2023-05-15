using ContactsManagement.Core.DTO.ContactsManager;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices
{
    /// <summary>
    /// Defines a service for retrieving person data from the system.
    /// </summary>
    public interface IPersonsGetterService
    {
        /// <summary>
        /// Retrieves list of person from the system.
        /// </summary>
        /// <param name="userId">The Id of the user who owns the data.</param>
        /// <returns>A list of person object with the corresponding UserId</returns>
        Task<List<PersonResponse>> GetAllPersons();
        /// <summary>
        /// Retrieves person from the system.
        /// </summary>
        /// <param name="personId">The request Id of the person to be retrived.</param>
        /// <returns>The person object with the corresponding Id</returns>
        Task<PersonResponse?> GetPersonById(Guid? personId);
        /// <summary>
        /// Retrieves person from the system.
        /// </summary>
        /// <param name="personIds">The list of Id of the person to be retrived.</param>
        /// <returns>List of person with the corresponding Id</returns>
        Task<List<PersonResponse>?> GetPersonsById(List<Guid>? personIds);
        /// <summary>
        /// Filters a list of persons based on a search text.
        /// </summary>
        /// <param name="userId">The Id of the user who owns the data.</param>
        /// <param name="searchProperty">The property of person to be filtered.</param>
        /// <param name="searchString">The search text to filter by.</param>
        /// <returns>A filtered list of persons.</returns>
        Task<List<PersonResponse>> GetFilteredPersons(string? searchProperty, string? searchString);
        /// <summary>
        /// Returns a CSV file as a MemoryStream containing the PersonResponse details of all persons belonging to a given user ID.
        /// </summary>
        /// <param name="userId">The Id of the user who owns the data.</param>
        /// <returns>A MemoryStream containing CSV data for all PersonResponse objects of the given user ID.</returns>
        Task<MemoryStream> GetPersonsCSV();
        /// <summary>
        /// Returns an excel file as a MemoryStream containing the PersonResponse details of all persons belonging to a given user ID.
        /// </summary>
        /// <param name="userId">The Id of the user who owns the data.</param>
        /// <returns>A MemoryStream containing excel data for all PersonResponse objects of the given user ID.</returns>
        Task<MemoryStream> GetPersonsEXCEL();
    }
}
