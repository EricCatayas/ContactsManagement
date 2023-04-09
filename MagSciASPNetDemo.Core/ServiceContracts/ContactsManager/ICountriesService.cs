using ContactsManagement.Core.DTO.ContactsManager;
using Microsoft.AspNetCore.Http;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager
{
    /// <summary>
    /// represents business logic for manipulating country entity
    /// </summary>
    public interface ICountriesService
    {
        Task<List<CountryResponse>> GetAllCountries();
    }
}