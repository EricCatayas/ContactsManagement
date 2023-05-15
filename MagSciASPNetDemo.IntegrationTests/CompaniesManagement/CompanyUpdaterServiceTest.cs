using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.CompaniesManagement;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.CompaniesManagement;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.CompaniesManagement;
using ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices;
using ContactsManagement.Core.Services.CompaniesManagement;
using ContactsManagement.Core.Services.ContactsManager.Persons;
using ContactsManagement.Infrastructure.DbContexts;
using ContactsManagement.Infrastructure.Repositories.CompaniesManagement;
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

namespace ContactsManagement.IntegrationTests.CompaniesManagement
{
    public class CompanyUpdaterServiceTest
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly Mock<ISignedInUserService> _mockSignedInUserService;
        public CompanyUpdaterServiceTest()
        {
            _loggerFactory = new LoggerFactory();
            _mockSignedInUserService = new Mock<ISignedInUserService>();
        }
        [Fact]
        public async void UpdateCompany_ReturnsUpdatedObject()
        {
            //Arrange
            Guid UserId = Guid.NewGuid();
            Company company = new()
            {
                UserId = UserId,
                CompanyId = 4534,
                CompanyName = "Sample",
                Industry = "Sample",
                CompanyDescription = "Sample",
                Address1 = "Sample",
                Address2 = "Sample",
                Employees = null,
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
            };
            CompanyUpdateRequest company_ToUpdate = new()
            {
                UserId = UserId,
                CompanyId = company.CompanyId,
                CompanyName = company.CompanyName,
                Industry = company.Industry,
                CompanyDescription = company.CompanyDescription,
                Address1 = company.Address1,
                Address2 = company.Address2,
                Employees = new List<Guid>() { person.Id, person2.Id },
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var personsGetterRepositoryLogger = _loggerFactory.CreateLogger<PersonsGetterRepository>();
                var companiesGetterRepositoryLogger = _loggerFactory.CreateLogger<CompaniesGetterRepository>();
                var companiesUpdaterRepositoryLogger = _loggerFactory.CreateLogger<CompaniesUpdaterRepository>();
                Mock<IDiagnosticContext> diagnosticContextMock = new Mock<IDiagnosticContext>();

                context.Persons.Add(person);
                context.Persons.Add(person2);
                context.Companies.Add(company);
                context.SaveChanges();

                IPersonsGetterRepository personsGetterRepository = new PersonsGetterRepository(context, personsGetterRepositoryLogger);
                ICompaniesUpdaterRepository companiesUpdaterRepository = new CompaniesUpdaterRepository(context, companiesUpdaterRepositoryLogger);
                ICompaniesGetterRepository companiesGetterRepository = new CompaniesGetterRepository(context, companiesGetterRepositoryLogger);
                ICompanyGetterService companyGetterService = new CompanyGetterService(companiesGetterRepository, _mockSignedInUserService.Object);
                ICompanyUpdaterService companyUpdaterService = new CompanyUpdaterService(companiesUpdaterRepository, personsGetterRepository, _mockSignedInUserService.Object);
                IPersonsGetterService personsGetterService = new PersonsGetterService(personsGetterRepository, diagnosticContextMock.Object, _mockSignedInUserService.Object);

                _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);
                //Act
                await companyUpdaterService.UpdateCompany(company_ToUpdate);
                var person1_WithCompany = await personsGetterService.GetPersonById(person.Id);
                var person2_WithCompany = await personsGetterService.GetPersonById(person2.Id);

                //Assert
                Assert.True(person1_WithCompany.CompanyName == company_ToUpdate.CompanyName &&
                            person1_WithCompany.CompanyId == company_ToUpdate.CompanyId);
                Assert.True(person2_WithCompany.CompanyName == company_ToUpdate.CompanyName &&
                            person2_WithCompany.CompanyId == company_ToUpdate.CompanyId);
            }
        }
    }
}
