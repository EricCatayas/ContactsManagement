using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices;
using ContactsManagement.Core.Services.ContactsManager.Persons;
using ContactsManagement.Infrastructure.DbContexts;
using ContactsManagement.Infrastructure.Repositories.ContactsManager.ContactGroups;
using ContactsManagement.Infrastructure.Repositories.ContactsManager.Persons;
using Microsoft.AspNetCore.Mvc.Filters;
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
    public class PersonsGetterServiceTest
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly Mock<ISignedInUserService> _mockSignedInUserService;
        public PersonsGetterServiceTest()
        {
            _loggerFactory = new LoggerFactory();
            _mockSignedInUserService = new Mock<ISignedInUserService>();
        }
        [Fact]
        public async void GetFilteredPersons_FilterByCompanyName_ToReturnObjects()
        {
            //Arrange
            Guid UserId = Guid.NewGuid();

            Company company = new()
            {
                CompanyId = 599,
                UserId = UserId,
                CompanyName = "Sample",
                Industry = "Sample",
                CompanyDescription = "Sample",
                Address1 = "Sample",
                Address2 = "Sample",
            };
            Person person = new()
            {
                UserId = UserId,
                Id = Guid.NewGuid(),
                Name = "Sample",
                Email = "Sample@email.com",
                Address = "Sample",
                DateOfBirth = new DateTime(2000, 12, 01),
            };
            Person person2 = new()
            {
                UserId = UserId,
                Id = Guid.NewGuid(),
                Name = "Sample",
                Email = "Sample@email.com",
                Address = "Sample",
                DateOfBirth = new DateTime(2000, 12, 01),
                CompanyId = company.CompanyId,
            };
            Person person3 = new()
            {
                UserId = UserId,
                Id = Guid.NewGuid(),
                Name = "Sample",
                Email = "Sample@email.com",
                Address = "Sample",
                DateOfBirth = new DateTime(2000, 12, 01),
                CompanyId = null
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var personsGetterRepositoryLogger = _loggerFactory.CreateLogger<PersonsGetterRepository>();
                Mock<IDiagnosticContext> diagnosticContextMock = new Mock<IDiagnosticContext>();

                context.Companies.Add(company);
                context.SaveChanges();
                context.Persons.Add(person);
                context.Persons.Add(person2);
                context.Persons.Add(person3);
                context.SaveChanges();

                IPersonsGetterRepository personsGetterRepository = new PersonsGetterRepository(context, personsGetterRepositoryLogger);
                IContactGroupsGetterRepository contactGroupsGetterRepository = new ContactGroupsGetterRepository(context);
                IPersonsGetterService personsGetterService = new PersonsGetterService(personsGetterRepository, diagnosticContextMock.Object, _mockSignedInUserService.Object);

                _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);
                //Act
                List<PersonResponse> persons_FromGetFilteredPersons = await personsGetterService.GetFilteredPersons(nameof(PersonResponse.CompanyName), company.CompanyName);
                //Assert
                Assert.True(persons_FromGetFilteredPersons.Count == 1);
                Assert.True(persons_FromGetFilteredPersons[0].CompanyName == company.CompanyName &&
                            persons_FromGetFilteredPersons[0].CompanyId == company.CompanyId);
            }
        }
        [Fact]
        public async void GetPersonsById_ToReturnObjects()
        {
            //Arrange
            Guid UserId = Guid.NewGuid();

            Person person = new()
            {
                Id = Guid.NewGuid(),
                Name = "Sample",
                Email = "Sample@email.com",
                Address = "Sample",
                DateOfBirth = new DateTime(2000, 12, 01),
            };
            Person person2 = new()
            {
                Id = Guid.NewGuid(),
                Name = "Sample",
                Email = "Sample@email.com",
                Address = "Sample",
                DateOfBirth = new DateTime(2000, 12, 01),
            };
            Person person3 = new()
            {
                Id = Guid.NewGuid(),
                Name = "Sample",
                Email = "Sample@email.com",
                Address = "Sample",
                DateOfBirth = new DateTime(2000, 12, 01),
                CompanyId = null
            };
            List<Guid> personIds_ToReturn = new List<Guid>() { person2.Id, person3.Id};

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var personsGetterRepositoryLogger = _loggerFactory.CreateLogger<PersonsGetterRepository>();
                Mock<IDiagnosticContext> diagnosticContextMock = new Mock<IDiagnosticContext>();

                context.Persons.Add(person);
                context.Persons.Add(person2);
                context.Persons.Add(person3);
                context.SaveChanges();

                IPersonsGetterRepository personsGetterRepository = new PersonsGetterRepository(context, personsGetterRepositoryLogger);
                IContactGroupsGetterRepository contactGroupsGetterRepository = new ContactGroupsGetterRepository(context);
                IPersonsGetterService personsGetterService = new PersonsGetterService(personsGetterRepository, diagnosticContextMock.Object, _mockSignedInUserService.Object);

                _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);

                //Act
                List<PersonResponse>? persons_FromGetPersonsById = await personsGetterService.GetPersonsById(personIds_ToReturn);
                //Assert
                Assert.True(persons_FromGetPersonsById.Count == personIds_ToReturn.Count);
            }
        }
    }
}
