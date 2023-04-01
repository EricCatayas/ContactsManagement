using ContactsManagement.Core.DTO.ContactsManager;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices
{
    public interface IPersonsGetterService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>returns all persons converted to PersonResponse object</returns>
        Task<List<PersonResponse>> GetAllPersons();
        Task<PersonResponse?> GetPersonById(Guid? personId);
        Task<List<PersonResponse>?> GetPersonsById(List<Guid>? personIDs);
        /// <summary>
        /// Returns matching List<Person> with matching given fied and search string 
        /// </summary>
        /// <param name="searchProperty">Search field to filter</param>
        /// <param name="searchString">Search string of filter</param>
        /// <returns>List of Persons of Matching Filter</returns>
        Task<List<PersonResponse>> GetFilteredPersons(string? searchProperty, string? searchString);
        Task<MemoryStream> GetPersonsCSV();
        /// <summary>
        /// Nuget EPPLus then configure the excelpackage licence in appsettings
        /// </summary>
        Task<MemoryStream> GetPersonsEXCEL();
    }
}
