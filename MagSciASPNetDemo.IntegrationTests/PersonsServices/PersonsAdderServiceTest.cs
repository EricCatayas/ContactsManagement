using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.CompaniesManagement;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.CompaniesManagement;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.Enums.ContactsManager;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.CompaniesManagement;
using ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices;
using ContactsManagement.Core.Services.CompaniesManagement;
using ContactsManagement.Core.Services.ContactsManager.Persons;
using ContactsManagement.Infrastructure.DbContexts;
using ContactsManagement.Infrastructure.Repositories.CompaniesManagement;
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
    public class PersonsAdderServiceTest
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly Mock<ISignedInUserService> _mockSignedInUserService;
        public PersonsAdderServiceTest()
        {
            _loggerFactory= new LoggerFactory();
            _mockSignedInUserService = new Mock<ISignedInUserService>();
        }
        [Fact]
        public async void AddPerson_WithCompanyId_ToReturnObjectWithCompanyDetails()
        {
            //Arrange
            Guid UserId = Guid.NewGuid();

            Company company = new()
            {
                CompanyId = 622,
                UserId= UserId,
                CompanyName = "Sample",
                Industry = "Sample",
                CompanyDescription = "Sample",
                Address1 = "Sample",
                Address2 = "Sample",
            };

            PersonAddRequest person_ToAdd = new PersonAddRequest()
            {
                PersonName = "Sample",
                Email = "sample@email.com",
                Address = "Sample",
                Gender = Gender.Get("Male"),
                DateOfBirth = new DateTime(2000, 12, 01),
                CompanyId = company.CompanyId,
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var personsAdderRepositoryLogger = _loggerFactory.CreateLogger<PersonsAdderRepository>();
                var personsGetterRepositoryLogger = _loggerFactory.CreateLogger<PersonsGetterRepository>();
                Mock<IDiagnosticContext> diagnosticContextMock = new Mock<IDiagnosticContext>();

                context.Companies.Add(company);
                context.SaveChanges();

                IPersonsAdderRepository personsAdderRepository = new PersonsAdderRepository(context, personsAdderRepositoryLogger);
                IPersonsGetterRepository personsGetterRepository = new PersonsGetterRepository(context, personsGetterRepositoryLogger);
                IContactGroupsGetterRepository contactGroupsGetterRepository = new ContactGroupsGetterRepository(context);
                IPersonsAdderService personsAdderService = new PersonsAdderService(personsAdderRepository, contactGroupsGetterRepository, _mockSignedInUserService.Object);
                IPersonsGetterService personsGetterService = new PersonsGetterService(personsGetterRepository, diagnosticContextMock.Object, _mockSignedInUserService.Object);

                _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);

                //Act
                PersonResponse? person_FromAddPerson = await personsAdderService.AddPerson(person_ToAdd);
                PersonResponse? person_FromGetPersonById = await personsGetterService.GetPersonById(person_FromAddPerson.PersonId);

                //Assert
                Assert.True(person_FromGetPersonById.CompanyName == company.CompanyName &&
                            person_FromGetPersonById.CompanyId == company.CompanyId);
            }
        }
        [Fact]
        public async void AddPerson_WithContactTag_ToReturnObjectWithContactTagDetails()
        {
            //Arrange
            Guid UserId = Guid.NewGuid();

            ContactTag contactTag = new()
            {
                TagId = 3947,
                TagName = "Sample",
                TagColor = "Sample",
                UserId = UserId
            };

            PersonAddRequest person_ToAdd = new PersonAddRequest()
            {
                PersonName = "Sample",
                Email = "sample@email.com",
                Address = "Sample",
                Gender = Gender.Get("Male"),
                DateOfBirth = new DateTime(2000, 12, 01),
                TagId = contactTag.TagId,
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var personsAdderRepositoryLogger = _loggerFactory.CreateLogger<PersonsAdderRepository>();
                var personsGetterRepositoryLogger = _loggerFactory.CreateLogger<PersonsGetterRepository>();
                Mock<IDiagnosticContext> diagnosticContextMock = new Mock<IDiagnosticContext>();

                context.ContactTags.Add(contactTag);
                context.SaveChanges();

                IPersonsAdderRepository personsAdderRepository = new PersonsAdderRepository(context, personsAdderRepositoryLogger);
                IPersonsGetterRepository personsGetterRepository = new PersonsGetterRepository(context, personsGetterRepositoryLogger);
                IContactGroupsGetterRepository contactGroupsGetterRepository = new ContactGroupsGetterRepository(context);
                IPersonsAdderService personsAdderService = new PersonsAdderService(personsAdderRepository, contactGroupsGetterRepository, _mockSignedInUserService.Object);
                IPersonsGetterService personsGetterService = new PersonsGetterService(personsGetterRepository, diagnosticContextMock.Object, _mockSignedInUserService.Object);

                _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);
                //Act
                PersonResponse? person_FromAddPerson = await personsAdderService.AddPerson(person_ToAdd);
                PersonResponse? person_FromGetPersonById = await personsGetterService.GetPersonById(person_FromAddPerson.PersonId);

                //Assert
                Assert.True(person_FromGetPersonById.Tag.TagId == contactTag.TagId &&
                            person_FromGetPersonById.Tag.TagName == contactTag.TagName &&
                            person_FromGetPersonById.Tag.TagColor == contactTag.TagColor);
            }
        }
        [Fact]
        public async void AddPerson_WithContactGroup_ToReturnObjectWithContactGroupDetails()
        {
            //Arrange
            Guid UserId = Guid.NewGuid();

            ContactGroup contactGroup = new()
            {
                GroupId = 1000,
                GroupName = "Sample",
                Description = "Sample",
                UserId= UserId
            };

            PersonAddRequest person_ToAdd = new PersonAddRequest()
            {
                PersonName = "Sample",
                Email = "sample@email.com",
                Address = "Sample",
                Gender = Gender.Get("Male"),
                DateOfBirth = new DateTime(2000, 12, 01),
                ContactGroups = new List<int> { contactGroup.GroupId }
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var personsAdderRepositoryLogger = _loggerFactory.CreateLogger<PersonsAdderRepository>();
                var personsGetterRepositoryLogger = _loggerFactory.CreateLogger<PersonsGetterRepository>();
                Mock<IDiagnosticContext> diagnosticContextMock = new Mock<IDiagnosticContext>();

                context.ContactGroups.Add(contactGroup);
                context.SaveChanges();

                IPersonsAdderRepository personsAdderRepository = new PersonsAdderRepository(context, personsAdderRepositoryLogger);
                IPersonsGetterRepository personsGetterRepository = new PersonsGetterRepository(context, personsGetterRepositoryLogger);
                IContactGroupsGetterRepository contactGroupsGetterRepository = new ContactGroupsGetterRepository(context);
                IPersonsAdderService personsAdderService = new PersonsAdderService(personsAdderRepository, contactGroupsGetterRepository, _mockSignedInUserService.Object);
                IPersonsGetterService personsGetterService = new PersonsGetterService(personsGetterRepository, diagnosticContextMock.Object, _mockSignedInUserService.Object);

                _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);
                //Act
                PersonResponse? person_FromAddPerson = await personsAdderService.AddPerson(person_ToAdd);
                PersonResponse? person_FromGetPersonById = await personsGetterService.GetPersonById(person_FromAddPerson.PersonId);

                //Assert
                Assert.True(person_FromGetPersonById.ContactGroups[0].GroupId == contactGroup.GroupId &&
                            person_FromGetPersonById.ContactGroups[0].GroupName == contactGroup.GroupName &&
                            person_FromGetPersonById.ContactGroups[0].Description == contactGroup.Description);
            }
        }
    }
}
