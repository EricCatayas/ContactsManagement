using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore;
using Moq;
using EntityFrameworkCoreMock;
using AutoFixture;
using FluentAssertions;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Serilog;
using ContactsManagement.Core.Exceptions.ContactsManager;
using ContactsManagement.Infrastructure.DbContexts;
using ContactsManagement.Core.ServiceContracts.ContactsManager;
using ContactsManagement.Core.Services.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.Enums.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices;
using Castle.Components.DictionaryAdapter.Xml;
using ContactsManagement.Infrastructure.Repositories.ContactsManager;
using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.CompaniesManagement;

namespace xUnitTesting
{
    /// <summary>
    /// Initial Properties of Person: 
    ///     Name, Id, Address, Gender, DateOfBirth, Email, CountryId, Country
    /// </summary>
    public class PersonServiceTest
    {
        private readonly IPersonsGetterService _personGetterService;
        private readonly IPersonsAdderService _personsAdderService;
        private readonly IPersonsDeleterService _personsDeleterService;
        private readonly IPersonsSorterService _personsSorterService;
        private readonly IPersonsUpdaterService _personsUpdaterService;
        private readonly ICountriesService _countriesService;
        private readonly IPersonsRepository _personsRepository;
        private readonly ICountriesRepository _countriesRepository;

        private readonly Mock<ICountriesRepository> _mockCountriesRepository; // <-- Mock methods

        private readonly Mock<IPersonsRepository> _mockPersonsRepository; // <-- Mock methods
        Mock<IPersonsAdderRepository> _mockPersonsAdderRepository;
        Mock<IPersonsGetterRepository> _mockPersonsGetterRepository;
        Mock<IPersonsUpdaterRepository> _mockPersonsUpdaterRepository;
        Mock<IPersonsDeleterRepository> _mockPersonsDeleterRepository;

        Mock<IContactGroupsGetterRepository> _mockContactGroupsGetterRepository;

        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;
        public PersonServiceTest(ITestOutputHelper testOutputHelper)
        {
            // Make DbSet<> virtual - so it can be overriden
            List<Country> initialCountriesData = new List<Country>() { };
            List<Person> initialPersonsData = new List<Person>() { };

            DbContextMock<ApplicationDbContext> mockDbContext = new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            ApplicationDbContext dbContext = mockDbContext.Object;
            mockDbContext.CreateDbSetMock(set => set.Countries, initialCountriesData);
            mockDbContext.CreateDbSetMock(set => set.Persons, initialPersonsData);

            _mockCountriesRepository = new Mock<ICountriesRepository>();
            _mockPersonsRepository = new Mock<IPersonsRepository>();
            _mockPersonsGetterRepository = new Mock<IPersonsGetterRepository>();
            _mockPersonsAdderRepository = new Mock<IPersonsAdderRepository>();
            _mockPersonsUpdaterRepository = new Mock<IPersonsUpdaterRepository>();
            _mockPersonsDeleterRepository = new Mock<IPersonsDeleterRepository>();
            Mock<ICompaniesRepository> _mockCompaniesRepository = new Mock<ICompaniesRepository>(); 

            _mockContactGroupsGetterRepository = new Mock<IContactGroupsGetterRepository>();
            Mock<ILogger<CountriesService>> _mockCountriesLogger = new Mock<ILogger<CountriesService>>();
            Mock<IDiagnosticContext> _mockDiagnosticContext = new Mock<IDiagnosticContext>();

            _personsRepository = _mockPersonsRepository.Object;
            _countriesRepository = _mockCountriesRepository.Object;

            _countriesService = new CountriesService(_countriesRepository, _mockCountriesLogger.Object); // You are using a dummy implementation -- 'new' is the orig
            _personGetterService = new PersonsGetterService(_mockPersonsGetterRepository.Object, _mockDiagnosticContext.Object);
            _personsDeleterService = new PersonsDeleterService(_mockPersonsDeleterRepository.Object, _mockPersonsGetterRepository.Object);
            _personsSorterService = new PersonsSorterService();
            _personsUpdaterService = new PersonsUpdaterService(_mockDiagnosticContext.Object, _mockPersonsUpdaterRepository.Object, _mockContactGroupsGetterRepository.Object);
            _testOutputHelper = testOutputHelper;
            _fixture = new Fixture();
        }
        #region AddPerson
        //When we supply null Person, it should throw ArgumentException
        [Fact]
        public void AddPerson_NullPersonAddRequest_ToBeArgumentNullException()
        {
            PersonAddRequest? nullPersonAddRequest = null;
            // Using Fluent Assertion
            Func<Task> action = async () =>
            {
                await _personsAdderService.AddPerson(nullPersonAddRequest);
            };
            action.Should().ThrowAsync<ArgumentNullException>();
        }
        [Fact]
        public async Task AddPerson_ValidPersonResponse_ToBeSuccesfull()
        {
            //Arrange
            Guid countryID= Guid.NewGuid();

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "jojobart",
                Email = "some@gmail.com",
                Address = "New York, City",
                Gender = Gender.Get("Male"),
                DateOfBirth = new DateTime(2000, 12, 01),
                CountryId = countryID,
            };
            Person person = personAddRequest.ToPerson();
            //Act
            //If we supply any argument value to the AddPerson method, it should return the same return value
            //You are mocking the method
            _mockPersonsRepository.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);
            PersonResponse? personResponse = await _personsAdderService.AddPerson(personAddRequest);

            //Assert
            Assert.NotNull(personResponse);
            Assert.True(personResponse.PersonName == personAddRequest.PersonName &&
                        personResponse.Gender.Equals(personAddRequest.Gender) &&
                        personResponse.Address == personAddRequest.Address &&
                        personResponse.DateOfBirth == personResponse.DateOfBirth &&
                        personResponse.CountryId == personResponse.CountryId &&
                        personResponse.Email == personResponse.Email);
        }

        //When we supply null value as PersonName, it should throw ArgumentException
        [Fact]
        public void AddPerson_NullProperties_ToBeArgumentException()
        {
            //Arrange
            var PersonAddRequest = new PersonAddRequest()
            {
                PersonName = null,
                Email = "something@gamila.com"
            };
            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _personsAdderService.AddPerson(PersonAddRequest);
            });
        }
        [Fact]
        public void AddPerson_InvalidDateOfBirth_ToBeArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                var PersonAddRequest = new PersonAddRequest()
                {
                    PersonName = "Jojoba",
                    Email = "Jojoba@outlook.com",
                    Address = "Heheheh",
                    DateOfBirth = DateTime.Now,
                    CountryId = Guid.NewGuid(),
                    Gender = Gender.Get("Female")
                };
                Person person = PersonAddRequest.ToPerson(); 
                _mockPersonsRepository.Setup(temp => temp.AddPerson(It.IsAny<Person>()))
                 .ReturnsAsync(person);
                await _personsAdderService.AddPerson(PersonAddRequest);
            });
        }
        #endregion
        #region GetAllPersons
        [Fact]
        public async  void GetAllPersons_RetrievablePerson_ToBeSuccessful()
        {
            Guid countryID = Guid.NewGuid();

            //Arrange
            List<Person> persons = new List<Person>() 
            {
                new Person()
                {
                    Name = "jojobart",
                    Email = "some@gmail.com",
                    Address = "New York, City",
                    Gender = "Male",
                    DateOfBirth = new DateTime(2000, 12, 01),
                    CountryId = countryID,
                },
                new Person()
                {
                    Name = "jojobart1",
                    Email = "some@gmail.com",
                    Address = "New York, City",
                    Gender = "Female",
                    DateOfBirth = new DateTime(2000, 12, 01),
                    CountryId = countryID,
                },
                new Person()
                {
                    Name = "jojobart2",
                    Email = "some@gmail.com",
                    Address = "New York, City",
                    Gender = "Other",
                    DateOfBirth = new DateTime(2000, 12, 01),
                    CountryId = countryID,
                },
            };

            List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();


            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_expected)
            {
                _testOutputHelper.WriteLine(person_response_from_add.PersonName);
            }

            _mockPersonsRepository.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);

            //Act
            List<PersonResponse> persons_list_from_get = await _personGetterService.GetAllPersons();

            //print persons_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_from_get in persons_list_from_get)
            {
                _testOutputHelper.WriteLine(person_response_from_get.PersonName);
            }

            //Assert
            persons_list_from_get.Should().BeEquivalentTo(person_response_list_expected);  //Fluent Assertion
        }
        [Fact]
        public async void GetAllPersons_EmptyList_ToBeEmpty()
        {
            //Arrange
            var persons = new List<Person>();
            _mockPersonsRepository
             .Setup(temp => temp.GetAllPersons())
             .ReturnsAsync(persons);
            //Assert
            List<PersonResponse> emptyList = await _personGetterService.GetAllPersons();
            Assert.Empty(emptyList);
        }
        #endregion


        #region GetPersonById
        [Fact]
        public async void GetPersonById_NullArgument_ToBeNull()
        {
            Guid? nullGuid = null;
            Assert.Null(await _personGetterService.GetPersonById(nullGuid));
        }
        [Fact]
        public async void GetPersonById_NonExistingPersonId_ToBeNull()
        {
            Guid? nullGuid = Guid.NewGuid();
            await Assert.ThrowsAsync<InvalidPersonIDException>( async () => {
                await _personGetterService.GetPersonById(nullGuid);
                });
        }
        [Fact]
        public async void GetPersonById_RetrievablePerson_ToBeSuccessful()
        {
            //Arrange
            CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();
            CountryResponse countryResponse = await _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(person => person.Email, "some@gmail.com")
                .With(person => person.PersonName, "JuanMan")
                .With(person => person.Gender, Gender.Get("Male"))
                .With(person => person.DateOfBirth, DateTime.Parse("2000-12-01"))
                .With(person => person.CountryId, countryResponse.CountryId).Create();

            var person = personAddRequest.ToPerson();
            _mockPersonsRepository
                .Setup(temp => temp.GetPersonById(It.IsAny<Guid>()))
                .ReturnsAsync(person);
            PersonResponse personExpected = person.ToPersonResponse();
            //Act
            PersonResponse? personResponse_from_GetPersonById = await _personGetterService.GetPersonById(person.Id);
            //Assert
            Assert.True(personExpected.Equals(personResponse_from_GetPersonById));
        }
        #endregion


        #region GetFilteredPersons
        [Fact]
        public async void GetFilteredPersons_EmptyList_ToBeEmpty()
        {
            List<Person> persons = new List<Person>() { };
            _mockPersonsRepository.Setup(temp => temp
              .GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
               .ReturnsAsync(persons);
            Assert.Empty(await _personGetterService.GetFilteredPersons("PersonName", "Jobart"));
        }
        [Fact]
        public async void GetFilteredPersons_EmptySearchText_ToBeSuccessful()
        {
            //Arrange
            Guid countryID = Guid.NewGuid();
            List<Person> persons = new List<Person>() {
            new Person()
                {
                    Name = "jojobart1",
                    Email = "some@gmail.com",
                    Address = "New York, City",
                    Gender = "Female",
                    DateOfBirth = new DateTime(2000, 12, 01),
                    CountryId = countryID,
                },

            new Person()
                {
                    Name = "jojobart1",
                    Email = "some@gmail.com",
                    Address = "New York, City",
                    Gender = "Female",
                    DateOfBirth = new DateTime(2000, 12, 01),
                    CountryId = countryID,
                },

            new Person()
                {
                    Name = "jojobart1",
                    Email = "some@gmail.com",
                    Address = "New York, City",
                    Gender = "Female",
                    DateOfBirth = new DateTime(2000, 12, 01),
                    CountryId = countryID,
                },
           };

            List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();


            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_expected)
            {
                _testOutputHelper.WriteLine(person_response_from_add.PersonName);
            }

            _mockPersonsRepository.Setup(temp => temp
            .GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
             .ReturnsAsync(persons);

            //Act
            List<PersonResponse> persons_list_from_search = await _personGetterService.GetFilteredPersons(nameof(PersonResponse.PersonName), "");

            //print persons_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_from_get in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_get.PersonName);
            }

            //Assert
            persons_list_from_search.Should().BeEquivalentTo(person_response_list_expected);
        
        }

        /// <summary>
        /// GetFilteredPersons must also return the Person with field value that contains substring of search string
        /// </summary>
        [Fact]
        public async void GetFilteredPersons_SearchByPersonName_ToBeSuccessful()
        {
            //Arrange
            Guid countryID = Guid.NewGuid();
            List<Person> persons = new List<Person>() {
            new Person()
                {
                    Name = "jojobart1",
                    Email = "some@gmail.com",
                    Address = "New York, City",
                    Gender = "Female",
                    DateOfBirth = new DateTime(2000, 12, 01),
                    CountryId = countryID,
                },

            new Person()
                {
                    Name = "love",
                    Email = "some@gmail.com",
                    Address = "New York, City",
                    Gender = "Female",
                    DateOfBirth = new DateTime(2000, 12, 01),
                    CountryId = countryID,
                },

            new Person()
                {
                    Name = "jojobart1",
                    Email = "some@gmail.com",
                    Address = "New York, City",
                    Gender = "Female",
                    DateOfBirth = new DateTime(2000, 12, 01),
                    CountryId = countryID,
                },
           };
            List<Person> persons_to_expect = new List<Person>()
            {
                new Person()
                {
                    Name = "love",
                    Email = "some@gmail.com",
                    Address = "New York, City",
                    Gender = "Female",
                    DateOfBirth = new DateTime(2000, 12, 01),
                    CountryId = countryID,
                },
            };

            List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();


            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_expected)
            {
                _testOutputHelper.WriteLine(person_response_from_add.PersonName);
            }

            _mockPersonsRepository.Setup(temp => temp
            .GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
             .ReturnsAsync(persons_to_expect);

            //Act
            List<PersonResponse> persons_list_from_search = await _personGetterService.GetFilteredPersons(nameof(PersonResponse.PersonName), "Love");

            //print persons_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_from_get in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_get.PersonName);
            }

            //Assert
            persons_list_from_search.Should().ContainSingle();
        }
        #endregion
        #region GetSortedPersons
        [Fact]
        public async void GetSortedPersons_DescendingOrder_ToBeSuccessful()
        {
            //Arrange
            Guid countryID = Guid.NewGuid();
            List<Person> persons = new List<Person>() {
            new Person()
                {
                    Name = "jojobart1",
                    Email = "some@gmail.com",
                    Address = "New York, City",
                    Gender = "Female",
                    DateOfBirth = new DateTime(2000, 12, 01),
                    CountryId = countryID,
                },

            new Person()
                {
                    Name = "jojobart3",
                    Email = "some@gmail.com",
                    Address = "New York, City",
                    Gender = "Female",
                    DateOfBirth = new DateTime(2000, 12, 01),
                    CountryId = countryID,
                },

            new Person()
                {
                    Name = "jojobart1",
                    Email = "some@gmail.com",
                    Address = "New York, City",
                    Gender = "Female",
                    DateOfBirth = new DateTime(2000, 12, 01),
                    CountryId = countryID,
                },
            };
            List<PersonResponse> persons_unordered = persons.Select(person => person.ToPersonResponse()).ToList();

            var persons_fromGetSorted = await _personsSorterService.GetSortedPersons(persons_unordered, "Name", SortOrderOptions.DESC);
            //Assert
            persons_fromGetSorted.Should().BeInDescendingOrder(person => person.PersonName);
        }
        [Fact]
        public async void GetSortedPersons_ReturnInAscendingOrder_ToBeSuccessful()
        {
            //Arrange
            Guid countryID = Guid.NewGuid();
            List<Person> persons = new List<Person>() {
            new Person()
                {
                    Name = "jojobart1",
                    Email = "some@gmail.com",
                    Address = "New York, City",
                    Gender = "Female",
                    DateOfBirth = new DateTime(2000, 12, 01),
                    CountryId = countryID,
                },

            new Person()
                {
                    Name = "jojobart3",
                    Email = "some@gmail.com",
                    Address = "New York, City",
                    Gender = "Female",
                    DateOfBirth = new DateTime(2000, 12, 01),
                    CountryId = countryID,
                },

            new Person()
                {
                    Name = "jojobart1",
                    Email = "some@gmail.com",
                    Address = "New York, City",
                    Gender = "Female",
                    DateOfBirth = new DateTime(2000, 12, 01),
                    CountryId = countryID,
                },
            };
            List<PersonResponse> persons_unordered = persons.Select(person => person.ToPersonResponse()).ToList();

            var persons_fromGetSorted = await _personsSorterService.GetSortedPersons(persons_unordered, "Name", SortOrderOptions.ASC);
            //Assert
            persons_fromGetSorted.Should().BeInAscendingOrder(person => person.PersonName);
        }
        #endregion
        #region UpdatePerson
        // If a null argument is passed, an ArgumentNullException must be thrown
        [Fact]
        public void UpdatePerson_NullArgument_ToBeArgumentNullException()
        {
            PersonUpdateRequest? nullRequest = null;
            Assert.ThrowsAsync<ArgumentNullException>(async() => await _personsUpdaterService.UpdatePerson(nullRequest));
        }
        // If a non-existent person Id passed, an ArgumentException must be thrown
        [Fact]
        public void UpdatePerson_NonExistentPersonId_ToBeArgumentException()
        {
            PersonUpdateRequest nonExistentPerson = new PersonUpdateRequest()
            {
                PersonId = Guid.NewGuid(),
                PersonName = "Love",
                Address = "Envy",
                Email = "Freedom@email.com",
                DateOfBirth = DateTime.Parse("2000-12-01"),
                CountryId = Guid.NewGuid(),
                Gender = GenderOptions.Male
            };

            Assert.ThrowsAsync<ArgumentException>(async () => await _personsUpdaterService.UpdatePerson(nonExistentPerson));
        }
        [Fact]
        public void UpdatePerson_InvalidProperties_ToBeArgumentException()
        {
            PersonUpdateRequest nonExistentPerson = new PersonUpdateRequest()
            {
                PersonId = Guid.NewGuid(),
                PersonName = "____",
                Address = "",
                Email = "not an email",
                DateOfBirth = DateTime.Parse("2000-12-01"),
                CountryId = Guid.NewGuid(),
                Gender = GenderOptions.Male
            };

            Assert.ThrowsAsync<ArgumentException>(async () => {
                try
                {
                    await _personsUpdaterService.UpdatePerson(nonExistentPerson);
                }catch(ArgumentException ex)
                {
                    _testOutputHelper.WriteLine(ex.Message);
                    throw new ArgumentException();
                }
                });
        }
        [Fact]
        public async void UpdatePerson_ValidPersonUpdate_ToBeSuccessful()
        {
            //Arrange
            Guid countryID = Guid.NewGuid();

            Person startingPerson = new Person()
            {
                Id = Guid.NewGuid(),
                Name = "jojobart",
                Email = "some@gmail.com",
                Address = "New York, City",
                Gender = "Male",
                DateOfBirth = new DateTime(2000, 12, 01),
                CountryId = countryID,
            };

            //Act
            PersonResponse person_NotUpdated = startingPerson.ToPersonResponse();
            PersonUpdateRequest person_expected = person_NotUpdated.ToPersonUpdateRequest();
            person_expected.Address = "Sesame Street";
            person_expected.PersonName = "Bojang Bogami";

            Person person_updated = person_expected.ToPerson();

            // _mockContactGroupsRepository.Setup(temp => temp.UpdateContactGroupsFromPerson(It.IsAny<Guid>(), It.IsAny<List<int>?>())).ReturnsAsync(person_updated);
            _mockPersonsRepository.Setup(temp => temp.UpdatePerson(It.IsAny<Person>())).ReturnsAsync(person_updated);

            PersonResponse updatedPerson_fromUpdatePerson = await _personsUpdaterService.UpdatePerson(person_expected);
            
            //Assert
            updatedPerson_fromUpdatePerson.Address.Should().NotBeEquivalentTo(startingPerson.Address);
            updatedPerson_fromUpdatePerson.PersonName.Should().NotBeEquivalentTo(startingPerson.Name);
        }
        #endregion
        #region DeletePerson
        [Fact]
        public void DeletePerson_NullArgument()
        {
            Guid nullId = new Guid();
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _personsDeleterService.DeletePerson(nullId));
        }
        [Fact]
        public async void DeletePerson_NonExistentPersonId()
        {
            Guid randomId = Guid.NewGuid();
            Assert.False(await _personsDeleterService.DeletePerson(randomId));
        }
        [Fact]
        public async void DeletePerson_PersonDeleted_ToBeSuccesful()
        {
            Guid countryID = Guid.NewGuid();

            Person person = new Person()
            {
                Name = "jojobart1",
                Email = "some@gmail.com",
                Address = "New York, City",
                Gender = "Female",
                DateOfBirth = new DateTime(2000, 12, 01),
                CountryId = countryID,

                JobTitle = "Janitor",
                ContactNumber1 = "434-9090",
                ContactNumber2 = "434-9090",

            };            


            _mockPersonsRepository.Setup(temp => temp.GetPersonById(It.IsAny<Guid>()))
             .ReturnsAsync(person);
            //This is stupid
            _mockPersonsRepository.Setup(temp => temp.DeletePerson(It.IsAny<Person>())).ReturnsAsync(true);
             
            Assert.True( await _personsDeleterService.DeletePerson(person.Id));
        }
        #endregion
    }
}
