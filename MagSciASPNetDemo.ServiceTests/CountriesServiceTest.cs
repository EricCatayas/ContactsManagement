using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using ContactsManagement.Core.ServiceContracts.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.Services.ContactsManager;
using ContactsManagement.Infrastructure.DbContexts;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.Domain.Entities.ContactsManager;

namespace xUnitTesting
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        private readonly ICountriesRepository _countriesRepository;
        private readonly Mock<ICountriesService> _countriesServiceMock;
        private readonly Mock<ICountriesRepository> _mockCountriesRepository;
        public CountriesServiceTest()
        {
            // Calling the mock functionality so that dbContext contains dummy implementation
            List<Country> initialCountriesData = new List<Country>() { };
            DbContextMock<ApplicationDbContext> mockDbContext = new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            ApplicationDbContext dbContext = mockDbContext.Object;
            mockDbContext.CreateDbSetMock(set => set.Countries, initialCountriesData);

            var loggerMock = new Mock<ILogger<CountriesService>>();
            _mockCountriesRepository = new Mock<ICountriesRepository>(); 
            _countriesRepository = _mockCountriesRepository.Object; 
            _countriesService = new CountriesService(_countriesRepository, loggerMock.Object);
        
        }

        #region AddCountry
        [Fact]
        public void AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = null;

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(async() =>
            {
                await _countriesService.AddCountry(countryAddRequest);
            });
        }
        [Fact]
        public void AddCountry_NullName() {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName= null,
            };
            //Assert
            Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                await _countriesService.AddCountry(countryAddRequest);
            });
        }
        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Philippines"
            };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest()
            {
                CountryName = "Philippines"
            };
            //Assert
            Assert.ThrowsAsync<ArgumentException>(async()=>{
                await _countriesService.AddCountry(countryAddRequest);
                await _countriesService.AddCountry(countryAddRequest2);
            });
        }
        #endregion
        #region GetAllCountries
        [Fact]
        public async void GetAllCountries_Emptylist()
        {
            List<Country> emptyList = new List<Country>() { };
            _mockCountriesRepository.Setup(temp => temp.GetAllCountries()).ReturnsAsync(emptyList);
            List<CountryResponse> ListOfCountryResponse = await _countriesService.GetAllCountries();

            Assert.Empty(ListOfCountryResponse);
        }
        [Fact]
        public async void GetAllCountries_RetrievableFromService_ToBeSuccessful()
        {
            //Arrange
            var country1 = new Country()
            {
                CountryId = Guid.NewGuid(),
                CountryName = "Test",
            };
            var country2 = new Country()
            {
                CountryId = Guid.NewGuid(),
                CountryName = "Test",
            };
            List<Country> countries = new();
            countries.Add(country1);
            countries.Add(country2);


            //Act
            _mockCountriesRepository.Setup(temp => temp.GetAllCountries()).ReturnsAsync(countries);
            List<CountryResponse> allCountryResponse= await _countriesService.GetAllCountries();

            //Assert
            allCountryResponse.Should().HaveCount(2);
            allCountryResponse.Should().Contain(country1.toCountryResponse());
            allCountryResponse.Should().Contain(country2.toCountryResponse());
        }
        #endregion
        #region GetCountryById
        [Fact]
        public async void GetCountryById_NullCountryId()
        {
            Guid? countryId = null;
            CountryResponse? country_response_from_get_method
            = await _countriesService.GetCountryById(countryId);
            Assert.Null(country_response_from_get_method);
        }
        [Fact]
        public async void GetCountryById_ValidReturnCountryResponse()
        {
            //Arrange
            var countryAddRequest = new CountryAddRequest() { CountryName="China"};
            var country = countryAddRequest.toCountry();
            var expectedCountry = country.toCountryResponse();

            _mockCountriesRepository.Setup(temp => temp.AddCountry(It.IsAny<Country>()))
                .ReturnsAsync(country);
            _mockCountriesRepository.Setup(temp => temp.GetCountryByCountryID(It.IsAny<Guid>()))
                .ReturnsAsync(country);

            CountryResponse countryResponse_from_addCountry = await _countriesService.AddCountry(countryAddRequest);
            //Act
            CountryResponse? countryResponse_from_GetCountryByID = await _countriesService.GetCountryById(country.CountryId);
            //Assert
            Assert.Equal(countryResponse_from_addCountry, countryResponse_from_GetCountryByID);


            Assert.Equal(expectedCountry, countryResponse_from_GetCountryByID);
        }
        [Fact]
        public async void GetCountryById_InvalidCountryId()
        {
            Guid countryId =  Guid.NewGuid();
            // Must return null if not found
            CountryResponse? country_response_from_GetCountryById = await _countriesService.GetCountryById(countryId);
            Assert.Null(country_response_from_GetCountryById);
        }
        #endregion
    }
}
