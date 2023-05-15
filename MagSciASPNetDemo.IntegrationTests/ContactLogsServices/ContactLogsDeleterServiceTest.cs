using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactLogsServices;
using ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices;
using ContactsManagement.Core.Services.ContactsManager.ContactLogs;
using ContactsManagement.Core.Services.ContactsManager.Persons;
using ContactsManagement.Infrastructure.DbContexts;
using ContactsManagement.Infrastructure.Repositories.ContactsManager.ContactGroups;
using ContactsManagement.Infrastructure.Repositories.ContactsManager.ContactLogs;
using ContactsManagement.Infrastructure.Repositories.ContactsManager.Persons;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.IntegrationTests.ContactLogsServices
{
    public class ContactLogsDeleterServiceTest
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly Mock<ISignedInUserService> _mockSignedInUserService;
        public ContactLogsDeleterServiceTest()
        {
            _loggerFactory = new LoggerFactory();
            _mockSignedInUserService = new Mock<ISignedInUserService>();
        }
        [Fact]
        public async void DeletePerson_WithLogReferences_ToDeleteContactLogs()
        {
            Guid UserId = Guid.NewGuid();

            Person person = new()
            {
                UserId = UserId,
                Id = Guid.NewGuid(),
                Name = "Sample",
                Email = "Sample@email.com",
                Address = "Sample",
                DateOfBirth = new DateTime(2000, 12, 01),
            };
            ContactLog contactLog = new()
            {
                PersonId = person.Id,
                UserId = UserId,
                LogId = 1000,
                LogTitle = "Sample",
                Note = "Sample",
                Type = "Sample"
            };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "test_database")
               .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var personsDeleterRepositoryLogger = _loggerFactory.CreateLogger<PersonsDeleterRepository>();
                var personsGetterRepositoryLogger = _loggerFactory.CreateLogger<PersonsGetterRepository>();
                Mock<IDiagnosticContext> diagnosticContextMock = new Mock<IDiagnosticContext>();

                context.Persons.Add(person);
                context.ContactLogs.Add(contactLog);
                context.SaveChanges();

                IPersonsDeleterRepository personsDeleterRepository = new PersonsDeleterRepository(context, personsDeleterRepositoryLogger);
                IPersonsGetterRepository personsGetterRepository = new PersonsGetterRepository(context, personsGetterRepositoryLogger);
                IContactGroupsGetterRepository contactGroupsGetterRepository = new ContactGroupsGetterRepository(context);
                IPersonsGetterService personsGetterService = new PersonsGetterService(personsGetterRepository, diagnosticContextMock.Object, _mockSignedInUserService.Object);
                IPersonsDeleterService personsDeleterService = new PersonsDeleterService(personsDeleterRepository, personsGetterRepository);

                _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);

                await personsDeleterService.DeletePerson(person.Id);

                Assert.True(!context.ContactLogs.Any(temp => temp.LogId == contactLog.LogId));
            }
        }
        [Fact]
        public async void DeleteContactLog_ToDeleteContactLog()
        {
            Guid UserId = Guid.NewGuid();
            ContactLog contactLog = new()
            {
                UserId = UserId,
                LogId = 1000,
                LogTitle = "Sample",
                Note = "Sample",
                Type = "Sample"
            };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "test_database")
               .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.ContactLogs.Add(contactLog);
                context.SaveChanges();

                IContactLogsDeleterRepository contactLogsDeleterRepository = new ContactLogsDeleterRepository(context);
                IContactLogsGetterRepository contactLogsGetterRepository = new ContactLogsGetterRepository(context);
                IContactLogsDeleterService contactLogsDeleterService = new ContactLogsDeleterService(contactLogsDeleterRepository, contactLogsGetterRepository, _mockSignedInUserService.Object);

                _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);

                await contactLogsDeleterService.DeleteContactLog(contactLog.LogId);

                Assert.True(!context.ContactLogs.Any(temp => temp.LogId == contactLog.LogId));
            }
        }
    }
}
