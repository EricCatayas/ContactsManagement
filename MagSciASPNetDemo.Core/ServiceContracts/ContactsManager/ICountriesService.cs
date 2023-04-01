using ContactsManagement.Core.DTO.ContactsManager;
using Microsoft.AspNetCore.Http;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager
{
    /// <summary>
    /// represents business logic for manipulating country entity
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Adds the country request object to the service
        /// </summary>
        /// <param name="countryAddRequest">the country that will be added to the service</param>
        /// <returns>so this is how intellisense reads the description</returns>
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);
        Task<List<CountryResponse>> GetAllCountries();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns>returns country object of matching CountryId, otherwise null.</returns>
        Task<CountryResponse?> GetCountryById(Guid? countryId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryName"></param>
        /// <returns>return country object of matching CountryName, otherwise null</returns>
        Task<CountryResponse?> GetCountryByCountryName(string? countryName);

        /// <summary>
        /// Uploads countries from Excel file to database
        /// </summary>
        /// <returns>Number of countries added</returns>
        Task<int> UploadCountriesFromExcelFile(IFormFile formFile);
    }
}