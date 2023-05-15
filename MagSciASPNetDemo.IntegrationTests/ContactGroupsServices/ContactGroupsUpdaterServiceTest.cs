using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactGroupsServices;
using ContactsManagement.Core.Services.ContactsManager.ContactGroups;
using ContactsManagement.Infrastructure.DbContexts;
using ContactsManagement.Infrastructure.Repositories.ContactsManager.ContactGroups;
using ContactsManagement.Infrastructure.Repositories.ContactsManager.Persons;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.IntegrationTests.ContactGroupsServices
{
    public class ContactGroupsUpdaterServiceTest
    {
        public readonly ILoggerFactory _loggerFactory;
        private readonly Mock<ISignedInUserService> _mockSignedInUserService;
        public ContactGroupsUpdaterServiceTest() { 
            _loggerFactory = new LoggerFactory();
            _mockSignedInUserService = new Mock<ISignedInUserService>();
        }
        [Fact]
        public async void UpdateContactGroup_ToReturnUpdatedObject()
        {
            Guid UserId = Guid.NewGuid();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "test_database")
               .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var personsGetterRepositoryLogger = _loggerFactory.CreateLogger<PersonsGetterRepository>();
                IContactGroupsUpdaterRepository contactGroupsUpdaterRepository = new ContactGroupsUpdaterRepository(context);

                IPersonsGetterRepository personsGetterRepository = new PersonsGetterRepository(context, personsGetterRepositoryLogger);
                IContactGroupsUpdaterService contactGroupsUpdaterService = new ContactGroupsUpdaterService(contactGroupsUpdaterRepository, personsGetterRepository, _mockSignedInUserService.Object);

                _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);

            }
        }
    }
}
