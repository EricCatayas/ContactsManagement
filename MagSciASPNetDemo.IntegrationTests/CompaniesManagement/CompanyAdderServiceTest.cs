using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.CompaniesManagement;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.CompaniesManagement;
using ContactsManagement.Core.DTO.ContactsManager;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ContactsManagement.IntegrationTests.CompaniesManagement
{
    public class CompanyAdderServiceTest
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly Mock<ISignedInUserService> _mockSignedInUserService;
        public CompanyAdderServiceTest()
        {
            _loggerFactory = new LoggerFactory();
            _mockSignedInUserService = new Mock<ISignedInUserService>();
        }
        [Fact]
        public async void AddCompany_WithEmployees_ToReturnObjectWithPersons()
        {
            //Arrange
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
            Person person2 = new()
            {
                UserId = UserId,
                Id = Guid.NewGuid(),
                Name = "Sample",
                Email = "Sample@email.com",
                Address = "Sample",
                DateOfBirth = new DateTime(2000, 12, 01),
            };
            CompanyAddRequest companyAddRequest = new()
            {
                CompanyName = "Sample",
                Industry = "Sample",
                CompanyDescription = "Sample",
                Address1 = "Sample",
                Address2 = "Sample",
                Employees = new List<Guid> { person.Id, person2.Id },
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var personsGetterRepositoryLogger = _loggerFactory.CreateLogger<PersonsGetterRepository>();
                var companiesAdderRepositoryLogger = _loggerFactory.CreateLogger<CompaniesAdderRepository>();

                context.Persons.Add(person);
                context.Persons.Add(person2);
                context.SaveChanges();

                IPersonsGetterRepository personsGetterRepository = new PersonsGetterRepository(context, personsGetterRepositoryLogger);
                ICompaniesAdderRepository companiesAdderRepository = new CompaniesAdderRepository(context, companiesAdderRepositoryLogger);
                ICompanyAdderService companyAdderService = new CompanyAdderService(companiesAdderRepository, personsGetterRepository, _mockSignedInUserService.Object);

                _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);
                //Act
                CompanyResponse company_FromAddCompany = await companyAdderService.AddCompany(companyAddRequest);

                //Assert
                Assert.True(company_FromAddCompany.Employees.Count == 2);
                Assert.True(company_FromAddCompany.Employees.Any(temp => temp.PersonId == person.Id));
                Assert.True(company_FromAddCompany.Employees.Any(temp => temp.PersonId == person2.Id));
            }
        }
    }
}
