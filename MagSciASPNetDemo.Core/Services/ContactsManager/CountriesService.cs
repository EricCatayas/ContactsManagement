using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Microsoft.Extensions.Logging;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager;
using ContactsManagement.Core.Domain.Entities.ContactsManager;

namespace ContactsManagement.Core.Services.ContactsManager
{
    public class CountriesService : ICountriesService
    {
        //private readonly ApplicationDbContext _personsDbContext;
        private readonly ICountriesRepository _countriesRepository;
        private readonly ILogger<CountriesService> _logger;
        public CountriesService(ICountriesRepository countriesRepository, ILogger<CountriesService> logger)
        {
            _countriesRepository = countriesRepository;
            _logger = logger;
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            List<Country> countries = await _countriesRepository.GetAllCountries();
            return countries.Select(country => country.toCountryResponse()).ToList();

        }      
    }
}