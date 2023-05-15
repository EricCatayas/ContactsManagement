using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.Enums.ContactsManager;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactTagsServices;
using ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices;
using ContactsManagement.Core.Services.ContactsManager.ContactTags;
using ContactsManagement.Core.Services.ContactsManager.Persons;
using ContactsManagement.Infrastructure.DbContexts;
using ContactsManagement.Infrastructure.Repositories.ContactsManager.ContactGroups;
using ContactsManagement.Infrastructure.Repositories.ContactsManager.ContactTags;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ContactsManagement.IntegrationTests.ContactTagsServices
{
    public class ContactTagsUpdaterServiceTest
    {
        public readonly ILoggerFactory _loggerFactory;
        private readonly Mock<ISignedInUserService> _mockSignedInUserService;
        public ContactTagsUpdaterServiceTest()
        {
            _loggerFactory = new LoggerFactory();
            _mockSignedInUserService = new Mock<ISignedInUserService>();
        }
        [Fact]
        public async void RemoveContactTagFromPerson_UpdatesObject()
        {
            Guid UserId = Guid.NewGuid();
            ContactTag contactTag = new()
            {
                TagId = 1000,
                TagName = "Test",
                TagColor = "Test",
                UserId = UserId
            };
            PersonAddRequest person_ToRemoveFromContactTag = new PersonAddRequest()
            {
                PersonName = "Sample",
                Email = "sample@email.com",
                Address = "Sample",
                Gender = Gender.Get("Male"),
                DateOfBirth = new DateTime(2000, 12, 01),
                TagId = contactTag.TagId
            };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "test_database")
               .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.ContactTags.Add(contactTag);
                context.SaveChanges();

                var personsAdderRepositoryLogger = _loggerFactory.CreateLogger<PersonsAdderRepository>();
                var personsGetterRepositoryLogger = _loggerFactory.CreateLogger<PersonsGetterRepository>();
                Mock<IDiagnosticContext> diagnosticContextMock = new Mock<IDiagnosticContext>();

                IPersonsAdderRepository personsAdderRepository = new PersonsAdderRepository(context, personsAdderRepositoryLogger);
                IPersonsGetterRepository personsGetterRepository = new PersonsGetterRepository(context, personsGetterRepositoryLogger);
                IContactGroupsGetterRepository contactGroupsGetterRepository = new ContactGroupsGetterRepository(context);
                IPersonsAdderService personsAdderService = new PersonsAdderService(personsAdderRepository, contactGroupsGetterRepository, _mockSignedInUserService.Object);
                IPersonsGetterService personsGetterService = new PersonsGetterService(personsGetterRepository, diagnosticContextMock.Object, _mockSignedInUserService.Object);
                IContactTagsUpdaterRepository contactTagsUpdaterRepository = new ContactTagsUpdaterRepository(context);
                IContactTagsUpdaterService contactTagsUpdaterService = new ContactTagsUpdaterService(contactTagsUpdaterRepository, personsGetterRepository);

                _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);

                PersonResponse person_FromAddPerson = await personsAdderService.AddPerson(person_ToRemoveFromContactTag);
                PersonResponse person_BeforeRemovalOfTag = await personsGetterService.GetPersonById(person_FromAddPerson.PersonId);

                bool isRemoved = await contactTagsUpdaterService.RemoveContactTagFromPerson(person_FromAddPerson.PersonId);

                PersonResponse person_AfterRemovalOfTag = await personsGetterService.GetPersonById(person_FromAddPerson.PersonId);

                Assert.True(person_BeforeRemovalOfTag.Tag != null && person_BeforeRemovalOfTag.TagId != null);
                Assert.True(isRemoved);
                Assert.True(person_AfterRemovalOfTag.Tag == null && person_AfterRemovalOfTag.TagId == null);
            }
        }
    }
}
