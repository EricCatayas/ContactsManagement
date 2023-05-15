using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.Enums.ContactsManager;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices;
using ContactsManagement.Core.Services.ContactsManager.Persons;
using ContactsManagement.Infrastructure.DbContexts;
using ContactsManagement.Infrastructure.Repositories.ContactsManager.ContactGroups;
using ContactsManagement.Infrastructure.Repositories.ContactsManager.Persons;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.IntegrationTests.PersonsServices
{
    public class PersonsUpdaterServiceTest
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly Mock<ISignedInUserService> _mockSignedInUserService;
        public PersonsUpdaterServiceTest()
        {
            _loggerFactory = new LoggerFactory();
            _mockSignedInUserService = new Mock<ISignedInUserService>();
        }
        [Fact]
        public async void UpdatePerson_NewCompanyId_ToReturnUpdatedPerson()
        {
            //Arrange
            Guid UserId = Guid.NewGuid();

            Company company = new()
            {
                CompanyId = 1000,
                UserId = UserId,
                CompanyName = "Sample",
                Industry = "Sample",
                CompanyDescription = "Sample",
                Address1 = "Sample",
                Address2 = "Sample",
            };
            Person person = new()
            {
                Id = Guid.NewGuid(),
                Name = "Sample",
                Email = "Sample@email.com",
                Address = "Sample",
                DateOfBirth = new DateTime(2000, 12, 01),
                CompanyId = null
            };

            PersonUpdateRequest person_ToUpdate = new PersonUpdateRequest()
            {
                PersonId = person.Id,
                PersonName = person.Name,
                Email = person.Email,
                Address = person.Address,
                DateOfBirth = person.DateOfBirth,
                CompanyId = company.CompanyId,
            };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var personsUpdaterRepositoryLogger = _loggerFactory.CreateLogger<PersonsUpdaterRepository>();
                var personsGetterRepositoryLogger = _loggerFactory.CreateLogger<PersonsGetterRepository>();
                Mock<IDiagnosticContext> diagnosticContextMock = new Mock<IDiagnosticContext>();

                context.Persons.Add(person);
                context.Companies.Add(company);
                context.SaveChanges();

                IPersonsUpdaterRepository personsUpdaterRepository = new PersonsUpdaterRepository(context, personsUpdaterRepositoryLogger);
                IPersonsGetterRepository personsGetterRepository = new PersonsGetterRepository(context, personsGetterRepositoryLogger);
                IContactGroupsGetterRepository contactGroupsGetterRepository = new ContactGroupsGetterRepository(context);
                IPersonsUpdaterService personsUpdaterService = new PersonsUpdaterService(personsUpdaterRepository, contactGroupsGetterRepository);
                IPersonsGetterService personsGetterService = new PersonsGetterService(personsGetterRepository, diagnosticContextMock.Object, _mockSignedInUserService.Object);

                _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);

                //Act
                _ = await personsUpdaterService.UpdatePerson(person_ToUpdate);
                PersonResponse updatedPerson_FromGetPersonById = await personsGetterService.GetPersonById(person_ToUpdate.PersonId);

                //Assert
                Assert.True(updatedPerson_FromGetPersonById.CompanyName == company.CompanyName &&
                            updatedPerson_FromGetPersonById.CompanyId == company.CompanyId);
            }
        }
        [Fact]
        public async void UpdatePerson_NewContactGroups_ToReturnUpdatedPerson()
        {
            //Arrange
            Guid UserId = Guid.NewGuid();
            ContactGroup contactGroup = new()
            {
                GroupId = 755,
                GroupName = "Sample",
                Description = "Sample",
                UserId = UserId
            };
            Person person = new()
            {
                Id = Guid.NewGuid(),
                Name = "Sample",
                Email = "Sample@email.com",
                Address = "Sample",
                DateOfBirth = new DateTime(2000, 12, 01),
                ContactGroups = null
            };

            PersonUpdateRequest person_ToUpdate = new PersonUpdateRequest()
            {
                PersonId = person.Id,
                PersonName = person.Name,
                Email = person.Email,
                Address = person.Address,
                DateOfBirth = person.DateOfBirth,
                ContactGroups = new List<int>() { contactGroup.GroupId }
            };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var personsUpdaterRepositoryLogger = _loggerFactory.CreateLogger<PersonsUpdaterRepository>();
                var personsGetterRepositoryLogger = _loggerFactory.CreateLogger<PersonsGetterRepository>();
                Mock<IDiagnosticContext> diagnosticContextMock = new Mock<IDiagnosticContext>();

                context.Persons.Add(person);
                context.ContactGroups.Add(contactGroup);
                context.SaveChanges();

                IPersonsUpdaterRepository personsUpdaterRepository = new PersonsUpdaterRepository(context, personsUpdaterRepositoryLogger);
                IPersonsGetterRepository personsGetterRepository = new PersonsGetterRepository(context, personsGetterRepositoryLogger);
                IContactGroupsGetterRepository contactGroupsGetterRepository = new ContactGroupsGetterRepository(context);
                IPersonsUpdaterService personsUpdaterService = new PersonsUpdaterService(personsUpdaterRepository, contactGroupsGetterRepository);
                IPersonsGetterService personsGetterService = new PersonsGetterService(personsGetterRepository, diagnosticContextMock.Object, _mockSignedInUserService.Object);

                _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);

                //Act
                _ = await personsUpdaterService.UpdatePerson(person_ToUpdate);
                PersonResponse updatedPerson_FromGetPersonById = await personsGetterService.GetPersonById(person_ToUpdate.PersonId);

                //Assert
                Assert.True(updatedPerson_FromGetPersonById.ContactGroups[0].GroupId == contactGroup.GroupId &&
                            updatedPerson_FromGetPersonById.ContactGroups[0].GroupName == contactGroup.GroupName &&
                            updatedPerson_FromGetPersonById.ContactGroups[0].Description == contactGroup.Description);
            }
        }
    }
}
