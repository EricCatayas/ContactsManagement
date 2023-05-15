using AutoFixture;
using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.Enums.ContactsManager;
using ContactsManagement.Core.Exceptions;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices;
using ContactsManagement.Core.Services.ContactsManager.Persons;
using ContactsManagement.Infrastructure.DbContexts;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace ContactsManagement.UnitTests.Persons
{
    public class PersonsGetterServiceTest
    {
        private readonly IPersonsGetterService _personGetterService;
        private readonly IPersonsSorterService _personsSorterService;

        private readonly Mock<IPersonsGetterRepository> _mockPersonsGetterRepository;
        private readonly Mock<ISignedInUserService> _mockSignedInUserService;

        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;
        public PersonsGetterServiceTest(ITestOutputHelper testOutputHelper)
        {
            _mockSignedInUserService = new Mock<ISignedInUserService>();
            _mockPersonsGetterRepository = new Mock<IPersonsGetterRepository>();
            Mock<IDiagnosticContext> _mockDiagnosticContext = new Mock<IDiagnosticContext>();

            _personGetterService = new PersonsGetterService(_mockPersonsGetterRepository.Object, _mockDiagnosticContext.Object, _mockSignedInUserService.Object);
            _personsSorterService = new PersonsSorterService();
            _fixture = new Fixture();
            _testOutputHelper = testOutputHelper;
        }

        #region GetAllPersons
        [Fact]
        public async void GetAllPersons_RetrievablePerson_ToBeSuccessful()
        {
            Guid UserId = Guid.NewGuid();
            int countryID = 1000;

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
                    UserId = UserId
                },
                new Person()
                {
                    Name = "jojobart1",
                    Email = "some@gmail.com",
                    Address = "New York, City",
                    Gender = "Female",
                    DateOfBirth = new DateTime(2000, 12, 01),
                    CountryId = countryID,
                    UserId = UserId
                },
                new Person()
                {
                    Name = "jojobart2",
                    Email = "some@gmail.com",
                    Address = "New York, City",
                    Gender = "Other",
                    DateOfBirth = new DateTime(2000, 12, 01),
                    CountryId = countryID,
                    UserId = UserId
                },
            };

            List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();


            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_expected)
            {
                _testOutputHelper.WriteLine(person_response_from_add.PersonName);
            }

            _mockPersonsGetterRepository.Setup(temp => temp.GetAllPersons(UserId)).ReturnsAsync(persons);
            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);

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
            Guid UserId = Guid.NewGuid();
            var persons = new List<Person>();

            _mockPersonsGetterRepository.Setup(temp => temp.GetAllPersons(UserId)).ReturnsAsync(persons);
            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);
            //Act
            List<PersonResponse> emptyList = await _personGetterService.GetAllPersons();
            //Assert
            Assert.Empty(emptyList);
        }
        [Fact]
        public async void GetAllPersons_NonSignedInUser_ToThrowAccessDeniedException()
        {
            //Arrange
            Guid UserId = Guid.NewGuid();
            var persons = new List<Person>();

            _mockPersonsGetterRepository.Setup(temp => temp.GetAllPersons(UserId)).ReturnsAsync(persons);
            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(null as Guid?);

            Func<Task> action = async () =>
            { 
                await _personGetterService.GetAllPersons();
            };
            //Assert
            await Assert.ThrowsAsync<AccessDeniedException>(action);
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
        public async void GetPersonById_NonExistingPersonId_ToThrowInvalidPersonIDException()
        {
            Guid? nullGuid = Guid.NewGuid();

            await Assert.ThrowsAsync<InvalidPersonIDException>(async () =>
            {
                await _personGetterService.GetPersonById(nullGuid);
            });
        }
        [Fact]
        public async void GetPersonById_RetrievablePerson_ToBeSuccessful()
        {
            //Arrange
            int countryID = 1000;

            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(person => person.Email, "some@gmail.com")
                .With(person => person.PersonName, "JuanMan")
                .With(person => person.Gender, Gender.Get("Male"))
                .With(person => person.DateOfBirth, DateTime.Parse("2000-12-01"))
                .With(person => person.CountryId, countryID).Create();

            var person = personAddRequest.ToPerson();
            _mockPersonsGetterRepository
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
            //Arrange
            Guid UserId = Guid.NewGuid();
            List<Person> persons = new List<Person>() { };
            //Act
            _mockPersonsGetterRepository.Setup(temp => temp
              .GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>(), It.IsAny<Guid>()))
              .ReturnsAsync(persons);

            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);
            //Assert
            Assert.Empty(await _personGetterService.GetFilteredPersons("PersonName", "Jobart"));
        }
        [Fact]
        public async void GetFilteredPersons_EmptySearchText_ToBeSuccessful()
        {
            //Arrange
            int countryID = 1000;
            Guid UserId = Guid.NewGuid();
            List<Person> persons = new List<Person>() {
            new Person()
                {
                    Name = "jojobart1",
                    Email = "some@gmail.com",
                    Address = "New York, City",
                    Gender = "Female",
                    DateOfBirth = new DateTime(2000, 12, 01),
                    CountryId = countryID,
                    UserId = UserId,
                },

            new Person()
                {
                    Name = "jojobart1",
                    Email = "some@gmail.com",
                    Address = "New York, City",
                    Gender = "Female",
                    DateOfBirth = new DateTime(2000, 12, 01),
                    CountryId = countryID,
                    UserId = UserId,
                },

            new Person()
                {
                    Name = "jojobart1",
                    Email = "some@gmail.com",
                    Address = "New York, City",
                    Gender = "Female",
                    DateOfBirth = new DateTime(2000, 12, 01),
                    CountryId = countryID,
                    UserId = UserId,
                },
           };

            List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();


            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_expected)
            {
                _testOutputHelper.WriteLine(person_response_from_add.PersonName);
            }

            _mockPersonsGetterRepository.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>(), It.IsAny<Guid>())).ReturnsAsync(persons);
            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);

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
            int countryID = 1000;
            Guid UserId = Guid.NewGuid();

            List<Person> persons = new List<Person>() {
            new Person()
                {
                    Name = "jojobart1",
                    Email = "some@gmail.com",
                    Address = "New York, City",
                    Gender = "Female",
                    DateOfBirth = new DateTime(2000, 12, 01),
                    CountryId = countryID,
                    UserId = UserId
                },

            new Person()
                {
                    Name = "love",
                    Email = "some@gmail.com",
                    Address = "New York, City",
                    Gender = "Female",
                    DateOfBirth = new DateTime(2000, 12, 01),
                    CountryId = countryID,
                    UserId = UserId
                },

            new Person()
                {
                    Name = "jojobart1",
                    Email = "some@gmail.com",
                    Address = "New York, City",
                    Gender = "Female",
                    DateOfBirth = new DateTime(2000, 12, 01),
                    CountryId = countryID,
                    UserId = UserId
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
                    UserId = UserId
                },
            };

            List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();


            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_expected)
            {
                _testOutputHelper.WriteLine(person_response_from_add.PersonName);
            }

            _mockPersonsGetterRepository.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>(), It.IsAny<Guid>())).ReturnsAsync(persons_to_expect);
            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);

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
            int countryID = 1000;
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
            int countryID = 1000;
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
    }
}
