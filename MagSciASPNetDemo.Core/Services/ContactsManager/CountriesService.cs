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
        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            //Validation: Arg must not be null
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }
            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException("Name of country must not be null");
            }
            if (await _countriesRepository.GetCountryByCountryName(countryAddRequest.CountryName) != null)
            {
                throw new ArgumentException("Country must not repeat");
            }

            Country countrytoAdd = countryAddRequest.toCountry();
            await _countriesRepository.AddCountry(countrytoAdd);
            return countrytoAdd.toCountryResponse();
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            List<Country> countries = await _countriesRepository.GetAllCountries();
            return countries.Select(country => country.toCountryResponse()).ToList();

        }

        public async Task<CountryResponse?> GetCountryByCountryName(string? countryName)
        {
            if (string.IsNullOrEmpty(countryName)) return null;

            Country? country = await _countriesRepository.GetCountryByCountryName(countryName);
            return country != null ? country.toCountryResponse() : null;
        }

        public async Task<CountryResponse?> GetCountryById(Guid? countryId)
        {
            if (countryId == null || countryId == Guid.Empty) return null;
            try
            {
                // "Throws ArgumentException if conditions not meet"
                Country? country = await _countriesRepository.GetCountryByCountryID((Guid)countryId);
                return country.toCountryResponse();
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// We'll send a file template to user,
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
        {
            MemoryStream memoryStream = new();

            await formFile.CopyToAsync(memoryStream);
            int countriesInserted = 0;

            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets["Countries"];

                int rowCount = workSheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string? cellValue = Convert.ToString(workSheet.Cells[row, 1].Value);

                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        string? countryName = cellValue;

                        if (await _countriesRepository.GetCountryByCountryName(countryName) != null)
                        {
                            Country country = new Country() { CountryName = countryName };
                            await _countriesRepository.AddCountry(country);

                            countriesInserted++;
                        }
                    }
                }
            }

            return countriesInserted;
        }
    }
}