using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.Enums.ContactsManager;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices;
using ContactsManagement.Core.Services.ContactsManager.Persons;
using ContactsManagement.Core.Exceptions;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.UnitTests.Persons
{
    public class PersonsAdderServiceTest
    {
        private readonly IPersonsAdderService _personsAdderService;
        private readonly Mock<IPersonsAdderRepository> _mockPersonsAdderRepository;
        private readonly Mock<ISignedInUserService> _mockSignedInUserService;
        private readonly Mock<IContactGroupsGetterRepository> _mockContactGroupsGetterRepository;
        public PersonsAdderServiceTest()
        { 
            _mockPersonsAdderRepository = new Mock<IPersonsAdderRepository>();
            _mockContactGroupsGetterRepository = new Mock<IContactGroupsGetterRepository>();
            _mockSignedInUserService = new Mock<ISignedInUserService>();    

            _personsAdderService = new PersonsAdderService(_mockPersonsAdderRepository.Object, _mockContactGroupsGetterRepository.Object, _mockSignedInUserService.Object);
        }
        #region AddPerson
        [Fact]
        public void AddPerson_NullPersonAddRequest_ToBeArgumentNullException()
        {
            //Arrange
            Guid UserId = Guid.NewGuid();
            PersonAddRequest? nullPersonAddRequest = null;

            //Act
            Func<Task> action = async () =>
            {
                await _personsAdderService.AddPerson(nullPersonAddRequest);
            };

            //Assert
            action.Should().ThrowAsync<ArgumentNullException>();
        }
        [Fact]
        public async Task AddPerson_ValidPersonResponse_ToBeSuccesfull()
        {
            //Arrange
            Guid UserId = Guid.NewGuid();
            int countryID = 1000;
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

            _mockPersonsAdderRepository.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);
            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);

            //Act
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
        [Fact]
        public async Task AddPerson_NonSignedInUser_ToThrowAccessDeniedException()
        {
            //Arrange
            Guid UserId = Guid.NewGuid();
            int countryID = 1000;
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

            _mockPersonsAdderRepository.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);
            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(null as Guid?);

            //Assert
            await Assert.ThrowsAsync<AccessDeniedException>(async () =>
            {
                _ = await _personsAdderService.AddPerson(personAddRequest);
            });
        }
        [Fact]
        public void AddPerson_NullProperties_ToBeArgumentException()
        {
            //Arrange
            Guid UserId = Guid.NewGuid();
            var PersonAddRequest = new PersonAddRequest()
            {
                PersonName = null,
                Email = "something@gamila.com"
            };

            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);
            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _personsAdderService.AddPerson(PersonAddRequest);
            });
        }
        [Fact]
        public void AddPerson_InvalidDateOfBirth_ToBeArgumentException()
        {
            //Arrange
            Guid UserId = Guid.NewGuid();
            var PersonAddRequest = new PersonAddRequest()
            {
                PersonName = "Jojoba",
                Email = "Jojoba@outlook.com",
                Address = "Heheheh",
                DateOfBirth = DateTime.Now,
                CountryId = 1000,
                Gender = Gender.Get("Female")
            };
            Person person = PersonAddRequest.ToPerson();
            _mockPersonsAdderRepository.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);
            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);

            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _personsAdderService.AddPerson(PersonAddRequest);
            });
        }
        #endregion
    }
}
