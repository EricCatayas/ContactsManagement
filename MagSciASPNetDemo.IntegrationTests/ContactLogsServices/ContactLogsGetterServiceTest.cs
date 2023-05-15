using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactLogsServices;
using ContactsManagement.Core.Services.ContactsManager.ContactLogs;
using ContactsManagement.Infrastructure.DbContexts;
using ContactsManagement.Infrastructure.Repositories.ContactsManager.ContactGroups;
using ContactsManagement.Infrastructure.Repositories.ContactsManager.ContactLogs;
using ContactsManagement.Infrastructure.Repositories.ContactsManager.Persons;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.IntegrationTests.ContactLogsServices
{ 
    public class ContactLogsGetterServiceTest
    {
        private readonly Mock<ISignedInUserService> _mockSignedInUserService;
        public ContactLogsGetterServiceTest()
        {
            _mockSignedInUserService = new Mock<ISignedInUserService>();
        }
        [Fact]
        public async void GetContactLogs_ToReturnContactLogsOfUser()
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
            ContactLog contactLog1_ToReturn = new ContactLog()
            {
                LogId = 6456,
                LogTitle= "Test",
                Note = "Test",
                DateCreated= DateTime.Now,
                Type = "Test",
                UserId = UserId,
                PersonId = person.Id,
            };
            ContactLog contactLog2_ToReturn = new ContactLog()
            {
                LogId = 5345,
                LogTitle = "Test",
                Note = "Test",
                DateCreated = DateTime.Now,
                Type = "Test",
                UserId = UserId,
                PersonId = person.Id,
            };
            ContactLog contactLog3_ToReturn = new ContactLog()
            {
                LogId = 1345,
                LogTitle = "Test",
                Note = "Test",
                DateCreated = DateTime.Now,
                Type = "Test",
                UserId = UserId,
                PersonId = person.Id,
            };
            ContactLog contactLog4_NoReturn = new ContactLog()
            {
                LogId = 4234,
                LogTitle = "Test",
                Note = "Test",
                DateCreated = DateTime.Now,
                Type = "Test",
                UserId = Guid.NewGuid(),
                PersonId = person.Id,
            };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "test_database")
               .Options;

            using (var context = new ApplicationDbContext(options))
            {                
                context.Persons.Add(person);
                context.ContactLogs.Add(contactLog1_ToReturn);
                context.ContactLogs.Add(contactLog2_ToReturn);
                context.ContactLogs.Add(contactLog3_ToReturn);
                context.ContactLogs.Add(contactLog4_NoReturn);
                context.SaveChanges();

                IContactLogsGetterRepository contactLogsGetterRepository = new ContactLogsGetterRepository(context);
                IContactLogsGetterService contactLogsGetterService = new ContactLogsGetterService(contactLogsGetterRepository, _mockSignedInUserService.Object);

                _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);

                List<ContactLogResponse>? contactLogs_FromGetContactLogs = await contactLogsGetterService.GetContactLogs();

                Assert.True(contactLogs_FromGetContactLogs.Count() == 3);
            }
        }
        [Fact]
        public async void GetFilteredContactLogs_ToReturnFilteredContactLogs()
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
            ContactLog contactLog1 = new ContactLog()
            {
                LogId = 1222,
                LogTitle = "Test",
                Note = "Test",
                DateCreated = DateTime.Now,
                Type = "Test",
                UserId = UserId,
                PersonId = person.Id,
            };
            ContactLog contactLog2 = new ContactLog()
            {
                LogId = 1333,
                LogTitle = "---",
                Note = "---",
                DateCreated = DateTime.Now,
                Type = "---",
                UserId = UserId,
                PersonId = person.Id,
            };
            ContactLog contactLog3 = new ContactLog()
            {
                LogId = 1444,
                LogTitle = "Test",
                Note = "Test",
                DateCreated = DateTime.Now,
                Type = "Test",
                UserId = UserId,
                PersonId = person.Id,
            };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "test_database")
               .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Persons.Add(person);
                context.ContactLogs.Add(contactLog1);
                context.ContactLogs.Add(contactLog2);
                context.ContactLogs.Add(contactLog3);
                context.SaveChanges();

                IContactLogsGetterRepository contactLogsGetterRepository = new ContactLogsGetterRepository(context);
                IContactLogsGetterService contactLogsGetterService = new ContactLogsGetterService(contactLogsGetterRepository, _mockSignedInUserService.Object);

                _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);

                List<ContactLogResponse> contactLogs_FromGetFilteredContactLogs = await contactLogsGetterService.GetFilteredContactLogs("Test");

                Assert.True(contactLogs_FromGetFilteredContactLogs.Count() == 2);
            }
        }
        [Fact]
        public async void GetContactLogByID()
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
            ContactLog contactLog_ToReturn = new ContactLog()
            {
                LogId = 2211,
                LogTitle = "Test",
                Note = "Test",
                DateCreated = DateTime.Now,
                Type = "Test",
                UserId = UserId,
                PersonId = person.Id,
            };
            ContactLog contactLog2 = new ContactLog()
            {
                LogId = 3311,
                LogTitle = "---",
                Note = "---",
                DateCreated = DateTime.Now,
                Type = "---",
                UserId = UserId,
                PersonId = person.Id,
            };
            ContactLog contactLog3 = new ContactLog()
            {
                LogId = 4411,
                LogTitle = "---",
                Note = "---",
                DateCreated = DateTime.Now,
                Type = "---",
                UserId = UserId,
                PersonId = person.Id,
            };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "test_database")
               .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Persons.Add(person);
                context.ContactLogs.Add(contactLog_ToReturn);
                context.ContactLogs.Add(contactLog2);
                context.ContactLogs.Add(contactLog3);
                context.SaveChanges();

                IContactLogsGetterRepository contactLogsGetterRepository = new ContactLogsGetterRepository(context);
                IContactLogsGetterService contactLogsGetterService = new ContactLogsGetterService(contactLogsGetterRepository, _mockSignedInUserService.Object);

                _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);

                ContactLogResponse contactLog_FromGetContactLogById = await contactLogsGetterService.GetContactLogById(contactLog_ToReturn.LogId);

                Assert.True(contactLog_FromGetContactLogById.LogId == contactLog_ToReturn.LogId &&
                            contactLog_FromGetContactLogById.LogTitle == contactLog_ToReturn.LogTitle &&
                            contactLog_FromGetContactLogById.DateCreated == contactLog_ToReturn.DateCreated &&
                            contactLog_FromGetContactLogById.Note == contactLog_ToReturn.Note);   
            }
        }
        [Fact]
        public async void GetContactLogsFromPerson()
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
            ContactLog contactLog1_ToRetrieve = new ContactLog()
            {
                LogId = 1211,
                LogTitle = "Test",
                Note = "Test",
                DateCreated = DateTime.Now,
                Type = "Test",
                UserId = UserId,
                PersonId = person.Id,
            };
            ContactLog contactLog2_ToRetrive = new ContactLog()
            {
                LogId = 1311,
                LogTitle = "Test",
                Note = "Test",
                DateCreated = DateTime.Now,
                Type = "Test",
                UserId = UserId,
                PersonId = person.Id,
            };
            ContactLog contactLog3 = new ContactLog()
            {
                LogId = 1411,
                LogTitle = "Test",
                Note = "Test",
                DateCreated = DateTime.Now,
                Type = "Test",
                UserId = UserId,
                PersonId = Guid.NewGuid(),
            };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "test_database")
               .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Persons.Add(person);
                context.ContactLogs.Add(contactLog1_ToRetrieve);
                context.ContactLogs.Add(contactLog2_ToRetrive);
                context.ContactLogs.Add(contactLog3);
                context.SaveChanges();

                IContactLogsGetterRepository contactLogsGetterRepository = new ContactLogsGetterRepository(context);
                IContactLogsGetterService contactLogsGetterService = new ContactLogsGetterService(contactLogsGetterRepository, _mockSignedInUserService.Object);

                _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);

                List<ContactLogResponse> contactLogs_FromGetContactLogsFromPerson = await contactLogsGetterService.GetContactLogsFromPerson(person.Id);

                Assert.True(contactLogs_FromGetContactLogsFromPerson.Count() == 2);
            }
        }
    }

}
