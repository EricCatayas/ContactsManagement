using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore;
using Moq;
using EntityFrameworkCoreMock;
using AutoFixture;
using FluentAssertions;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Serilog;
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
using ContactsManagement.Core.Services.ContactsManager.Persons;
using ContactsManagement.Core.Exceptions;

namespace ContactsManagement.UnitTests.Persons
{
    /// <summary>
    /// Initial Properties of Person: 
    ///     Name, Id, Address, Gender, DateOfBirth, Email, CountryId, Country
    /// </summary>
    public class PersonsUpdaterServiceTest
    {
        
        private readonly IPersonsUpdaterService _personsUpdaterService;

        private readonly Mock<IPersonsUpdaterRepository> _mockPersonsUpdaterRepository;
        private readonly Mock<IContactGroupsGetterRepository> _mockContactGroupsGetterRepository;
        private readonly ITestOutputHelper _testOutputHelper;
        public PersonsUpdaterServiceTest(ITestOutputHelper testOutputHelper)
        {
            _mockPersonsUpdaterRepository = new Mock<IPersonsUpdaterRepository>();

            _mockContactGroupsGetterRepository = new Mock<IContactGroupsGetterRepository>();
                        
            _personsUpdaterService = new PersonsUpdaterService(_mockPersonsUpdaterRepository.Object, _mockContactGroupsGetterRepository.Object);
            _testOutputHelper = testOutputHelper;
        }                

        #region UpdatePerson
        // If a null argument is passed, an ArgumentNullException must be thrown
        [Fact]
        public void UpdatePerson_NullArgument_ToBeArgumentNullException()
        {
            PersonUpdateRequest? nullRequest = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _personsUpdaterService.UpdatePerson(nullRequest));
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
                CountryId = 1000,
                Gender = GenderOptions.Male,
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
                CountryId = 1000,
                Gender = GenderOptions.Male
            };

            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                try
                {
                    await _personsUpdaterService.UpdatePerson(nonExistentPerson);
                }
                catch (ArgumentException ex)
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
            int countryID = 1000;

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
            _mockPersonsUpdaterRepository.Setup(temp => temp.UpdatePerson(It.IsAny<Person>())).ReturnsAsync(person_updated);

            PersonResponse updatedPerson_fromUpdatePerson = await _personsUpdaterService.UpdatePerson(person_expected);

            //Assert
            updatedPerson_fromUpdatePerson.Address.Should().NotBeEquivalentTo(startingPerson.Address);
            updatedPerson_fromUpdatePerson.PersonName.Should().NotBeEquivalentTo(startingPerson.Name);
        }
        #endregion       
    }
}
