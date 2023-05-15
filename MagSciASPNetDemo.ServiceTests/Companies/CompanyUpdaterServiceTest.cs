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
    public class CompanyUpdaterServiceTest
    {
        private readonly ICompanyUpdaterService _companyUpdaterService;

        private readonly Mock<ICompaniesUpdaterRepository> _companiesUpdaterRepositoryMock;
        private readonly Mock<IPersonsGetterRepository> _personsGetterRepositoryMock;
        private readonly Mock<ISignedInUserService> _mockSignedInUserService;
        public CompanyUpdaterServiceTest()
        {
            _companiesUpdaterRepositoryMock = new Mock<ICompaniesUpdaterRepository>();
            _personsGetterRepositoryMock = new Mock<IPersonsGetterRepository>();
            _mockSignedInUserService= new Mock<ISignedInUserService>();

            _companyUpdaterService = new CompanyUpdaterService(_companiesUpdaterRepositoryMock.Object, _personsGetterRepositoryMock.Object, _mockSignedInUserService.Object);
        }
        [Fact]
        public async void UpdateCompany_NonSignedInUser_ToThrowAccessDeniedException()
        {
            //Assert
            CompanyUpdateRequest company_ToUpdate = new()
            {
                UserId = null,
                CompanyName = "Sample",
                Industry = "Sample",
                CompanyDescription = "Sample",
                Address1 = "Sample",
                Address2 = "Sample",
            };
            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(null as Guid?);
            //Assert
            await Assert.ThrowsAsync<AccessDeniedException>(async () =>
            {
                await _companyUpdaterService.UpdateCompany(company_ToUpdate);
            });
        }
    }
}
