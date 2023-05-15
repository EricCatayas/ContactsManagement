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
    public class CompanyAdderByNameServiceTest
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly Mock<ISignedInUserService> _mockSignedInUserService;
        public CompanyAdderByNameServiceTest()
        {
            _loggerFactory = new LoggerFactory();
            _mockSignedInUserService = new Mock<ISignedInUserService>();
        }
        [Fact]
        public async void AddCompany_ValidInput_ToBeSuccessful()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            Guid UserId = Guid.NewGuid();
            const string companyName = "Sample Test"; 

            using (var context = new ApplicationDbContext(options))
            {
                var companiesAdderRepositoryLogger = _loggerFactory.CreateLogger<CompaniesAdderRepository>();
                var companiesGetterRepositoryLogger = _loggerFactory.CreateLogger<CompaniesGetterRepository>();
                
                ICompaniesAdderRepository companiesAdderRepository = new CompaniesAdderRepository(context, companiesAdderRepositoryLogger);
                ICompaniesGetterRepository companiesGetterRepository = new CompaniesGetterRepository(context, companiesGetterRepositoryLogger);
                ICompanyAdderByNameService companyAdderByNameService = new CompanyAdderByNameService(companiesAdderRepository, _mockSignedInUserService.Object);
                ICompanyGetterService companyGetterService = new CompanyGetterService(companiesGetterRepository, _mockSignedInUserService.Object);

                _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);

                int companyId = await companyAdderByNameService.AddCompanyByName(companyName);
                CompanyResponse? companyAdded = await companyGetterService.GetCompanyById(companyId);

                Assert.NotNull(companyAdded);
                Assert.True(companyAdded.CompanyName == companyName);
            }
        }
    }
}
