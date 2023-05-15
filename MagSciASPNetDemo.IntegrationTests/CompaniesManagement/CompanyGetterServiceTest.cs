using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.CompaniesManagement;
using ContactsManagement.Core.DTO.CompaniesManagement;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.CompaniesManagement;
using ContactsManagement.Core.Services.CompaniesManagement;
using ContactsManagement.Infrastructure.DbContexts;
using ContactsManagement.Infrastructure.Repositories.CompaniesManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.IntegrationTests.CompaniesManagement
{
    public class CompanyGetterServiceTest
    {
        public readonly ILoggerFactory _loggerFactory;
        private readonly Mock<ISignedInUserService> _mockSignedInUserService;
        public CompanyGetterServiceTest() 
        {
            _loggerFactory = new LoggerFactory();
            _mockSignedInUserService = new Mock<ISignedInUserService>();
        }
        [Fact]
        public async void GetAllCompanies_ToReturnAllCompaniesOfUser()
        {
            Guid UserId = Guid.NewGuid();
            Company company1_ToReturn = new()
            {
                CompanyId = 5234,
                UserId = UserId,
                CompanyName = "Sample",
                Industry = "Sample",
                CompanyDescription = "Sample",
                Address1 = "Sample",
                Address2 = "Sample",
            };
            Company company2_ToReturn = new()
            {
                CompanyId = 7438,
                UserId = UserId,
                CompanyName = "Sample",
                Industry = "Sample",
                CompanyDescription = "Sample",
                Address1 = "Sample",
                Address2 = "Sample",
            };
            Company company3_NoReturn = new()
            {
                CompanyId = 6598,
                UserId = Guid.NewGuid(),
                CompanyName = "Sample",
                Industry = "Sample",
                CompanyDescription = "Sample",
                Address1 = "Sample",
                Address2 = "Sample",
            };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "test_database")
               .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Companies.Add(company1_ToReturn);
                context.Companies.Add(company2_ToReturn);
                context.Companies.Add(company3_NoReturn);
                context.SaveChanges();

                var companiesGetterRepositoryLogger = _loggerFactory.CreateLogger<CompaniesGetterRepository>();
                ICompaniesGetterRepository companiesGetterRepository = new CompaniesGetterRepository(context, companiesGetterRepositoryLogger);
                ICompanyGetterService companyGetterService = new CompanyGetterService(companiesGetterRepository, _mockSignedInUserService.Object);

                _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);

                List<CompanyResponse>? companies_FromUser = await companyGetterService.GetAllCompanies();
                Assert.True(companies_FromUser.Count() == 2);
            }
        }
        [Fact]
        public async void GetCompanyById_ToReturnCompany()
        {
            Guid UserId = Guid.NewGuid();
            Company company1_ToReturn = new()
            {
                CompanyId = 6534,
                UserId = UserId,
                CompanyName = "Sample",
                Industry = "Sample",
                CompanyDescription = "Sample",
                Address1 = "Sample",
                Address2 = "Sample",
            };
            Company company2_NoReturn = new()
            {
                CompanyId = 7438,
                UserId = UserId,
                CompanyName = "Sample",
                Industry = "Sample",
                CompanyDescription = "Sample",
                Address1 = "Sample",
                Address2 = "Sample",
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "test_database")
               .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Companies.Add(company1_ToReturn);
                context.Companies.Add(company2_NoReturn);
                context.SaveChanges();

                var companiesGetterRepositoryLogger = _loggerFactory.CreateLogger<CompaniesGetterRepository>();
                ICompaniesGetterRepository companiesGetterRepository = new CompaniesGetterRepository(context, companiesGetterRepositoryLogger);
                ICompanyGetterService companyGetterService = new CompanyGetterService(companiesGetterRepository, _mockSignedInUserService.Object);

                _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);

                CompanyResponse? company_FromGetCompanyById = await companyGetterService.GetCompanyById(company1_ToReturn.CompanyId);

                Assert.True(company1_ToReturn.CompanyId == company_FromGetCompanyById.CompanyId);
            }
        }
    }
}
