using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.CompaniesManagement;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.CompaniesManagement;
using ContactsManagement.Core.Exceptions;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.CompaniesManagement;
using ContactsManagement.Core.Services.CompaniesManagement;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.UnitTests.Companies
{
    public class CompanyAdderServiceTest
    {
        private readonly ICompanyAdderService _companyAdderService;
        private readonly Mock<ISignedInUserService> _mockSignedInUserService;
        private readonly Mock<ICompaniesAdderRepository> _companiesAdderRepositoryMock;
        private readonly Mock<IPersonsGetterRepository> _personsGetterRepositoryMock;

        public CompanyAdderServiceTest()
        {
            _companiesAdderRepositoryMock = new Mock<ICompaniesAdderRepository>();
            _personsGetterRepositoryMock = new Mock<IPersonsGetterRepository>();
            _mockSignedInUserService= new Mock<ISignedInUserService>();

            _companyAdderService = new CompanyAdderService(_companiesAdderRepositoryMock.Object, _personsGetterRepositoryMock.Object, _mockSignedInUserService.Object);
        }

        [Fact]
        public async void AddCompany_NonSignedInUser_ToThrowAccessDeniedException()
        {
            //Arrange
            Guid UserID = Guid.NewGuid();
            CompanyAddRequest companyAddRequest = new()
            {
                CompanyName = "Sample",
                Industry = "Sample",
                CompanyDescription = "Sample",
                Address1 = "Sample",
                Address2 = "Sample",                
            };
            Company companyToReturn = companyAddRequest.ToCompany();

            List<Person>? emptyPersonsList = new(); 
            _personsGetterRepositoryMock.Setup(temp => temp.GetPersonsById(It.IsAny<List<Guid>>())).ReturnsAsync(emptyPersonsList);
            _companiesAdderRepositoryMock.Setup(temp => temp.AddCompany(It.IsAny<Company>())).ReturnsAsync(companyToReturn); 
            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(null as Guid?);

            //Act
            Func<Task> action = async () =>
            {
                await _companyAdderService.AddCompany(companyAddRequest);
            };

            Assert.ThrowsAsync<AccessDeniedException>(() => action());
        }
        [Fact]
        public async void AddCompany_ValidArgument_ToReturnWithCompanyId()
        {
            //Arrange
            Guid UserID = Guid.NewGuid();
            CompanyAddRequest companyAddRequest = new()
            {
                CompanyName = "Sample",
                Industry = "Sample",
                CompanyDescription = "Sample",
                Address1 = "Sample",
                Address2 = "Sample",
            };
            Company companyToReturn = companyAddRequest.ToCompany();

            List<Person>? emptyPersonsList = new();
            _personsGetterRepositoryMock.Setup(temp => temp.GetPersonsById(It.IsAny<List<Guid>>())).ReturnsAsync(emptyPersonsList);
            _companiesAdderRepositoryMock.Setup(temp => temp.AddCompany(It.IsAny<Company>())).ReturnsAsync(companyToReturn);
            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserID);

            //Act
            CompanyResponse companyResponse = await _companyAdderService.AddCompany(companyAddRequest);

            Assert.NotNull(companyResponse.CompanyId);
        }
    }
}
