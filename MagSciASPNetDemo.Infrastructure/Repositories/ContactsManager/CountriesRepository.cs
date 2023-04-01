using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace ContactsManagement.Infrastructure.Repositories.ContactsManager
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<CountriesRepository> _logger; // W/out '<T>', injection will run through trouble
        public CountriesRepository(ApplicationDbContext db, ILogger<CountriesRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Country> AddCountry(Country country)
        {
            _logger.LogDebug("AddCountry in CountriesRepository");
            _db.Countries.Add(country);
            await _db.SaveChangesAsync();

            return country;
        }

        public async Task<List<Country>> GetAllCountries()
        {
            _logger.LogDebug("GetAllCountries in CountriesRepository");
            return await _db.Countries.ToListAsync();
        }

        public async Task<Country?> GetCountryByCountryID(Guid countryID)
        {
            _logger.LogDebug("GetCountryByCountryId in CountriesRepository");
            return await _db.Countries.FirstOrDefaultAsync(temp => temp.CountryId == countryID);
        }

        public async Task<Country?> GetCountryByCountryName(string countryName)
        {
            _logger.LogDebug("GetCountryByCountryName in CountriesRepository");
            return await _db.Countries.FirstOrDefaultAsync(temp => temp.CountryName == countryName);
        }
    }
}