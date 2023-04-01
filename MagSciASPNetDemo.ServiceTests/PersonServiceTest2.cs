using AutoFixture;
using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices;
using EntityFrameworkCoreMock;
using FluentAssertions;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.Enums.ContactsManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager;
using ContactsManagement.Core.Services.ContactsManager;
using ContactsManagement.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using ContactsManagement.Infrastructure.Repositories.CompaniesManagement;
using ContactsManagement.Core.Domain.RepositoryContracts.CompaniesManagement;
using ContactsManagement.Infrastructure.Repositories.ContactsManager.ContactGroups;
using ContactsManagement.Infrastructure.Repositories.ContactsManager.ContactTags;
using ContactsManagement.Infrastructure.Repositories.ContactsManager.Persons;

namespace xUnitTesting
{
    /// <summary>
    /// Expansion of ContactsManagement:
    ///     Jobtitle, CompanyId, Company, ProfileBlobUrl, Relationship, , ContactNumbers, ContactGroups, ContactTags, ContactNotes, ContactLogs
    /// </summary>
    public class PersonServiceTest2
    {
        private readonly IPersonsGetterService _personGetterService;
        private readonly IPersonsAdderService _personsAdderService;
        private readonly IPersonsDeleterService _personsDeleterService;
        private readonly IPersonsSorterService _personsSorterService;
        private readonly IPersonsUpdaterService _personsUpdaterService;
        private readonly IPersonsRepository _personsRepository;

        DbContextMock<ApplicationDbContext> _mockDbContext;
        Mock<IPersonsAdderRepository> _mockPersonsAdderRepository;
        Mock<IPersonsGetterRepository> _mockPersonsGetterRepository;
        Mock<IPersonsUpdaterRepository> _mockPersonsUpdaterRepository;
        Mock<IPersonsDeleterRepository> _mockPersonsDeleterRepository;
        //Mock<IContactTagsRepository> _mockContactTagsRepository;
        Mock<IContactGroupsGetterRepository> _mockContactGroupsGetterRepository;
        Mock<IDiagnosticContext> _mockDiagnosticContext;


        private readonly ITestOutputHelper _testOutputHelper;
        public PersonServiceTest2(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            List<Person> initialPersonsData = new List<Person>() { };
            List<Company> initialCompaniesData = new List<Company>() {
                new Company()
                {
                    CompanyId = 1,
                    CompanyName = "Test",
                }
            };
            List<ContactTag> initialContactTagsData = new List<ContactTag>()
            {

            };
            List<ContactGroup> initialContactGroupsData = new List<ContactGroup>() { };

            _mockDbContext = new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            _mockDbContext.CreateDbSetMock(set => set.Persons, initialPersonsData);
            _mockDbContext.CreateDbSetMock(set => set.Companies, initialCompaniesData);
            _mockDbContext.CreateDbSetMock(set => set.ContactTags, initialContactTagsData);
            _mockDbContext.CreateDbSetMock(set => set.ContactGroups, initialContactGroupsData);
            ApplicationDbContext dbContext = _mockDbContext.Object;


            _mockPersonsGetterRepository = new Mock<IPersonsGetterRepository>();
            _mockPersonsAdderRepository = new Mock<IPersonsAdderRepository>();
            _mockPersonsUpdaterRepository = new Mock<IPersonsUpdaterRepository>();
            _mockPersonsDeleterRepository = new Mock<IPersonsDeleterRepository>();
            //_mockContactTagsRepository = new Mock<IContactTagsRepository>();
            _mockContactGroupsGetterRepository = new Mock<IContactGroupsGetterRepository>();
            _mockDiagnosticContext = new Mock<IDiagnosticContext>();

            // Mock<ILoggers>
            var _ILoggerPersons = new Mock<ILogger<PersonsRepository>>();
            var _ILoggerCompanies = new Mock<ILogger<CompaniesRepository>>();
            var _ILoggerContactGroups = new Mock<ILogger<ContactGroupsRepository>>();
            //If ApplicationDbContext is mocked only
            IPersonsRepository personsRepository = new PersonsRepository(dbContext, _ILoggerPersons.Object);
            ICompaniesRepository companiesRepository = new CompaniesRepository(dbContext, _ILoggerCompanies.Object);
            //IContactTagsRepository contactTagsRepository = new ContactTagsRepository(dbContext, _ILoggerContactTags.Object);
            //IContactGroupsRepository contactGroupsRepository = new ContactGroupsRepository(dbContext, _ILoggerContactGroups.Object);

            _personsAdderService = new PersonsAdderService(_mockDiagnosticContext.Object, _mockPersonsAdderRepository.Object, _mockContactGroupsGetterRepository.Object);
            _personGetterService = new PersonsGetterService(_mockPersonsGetterRepository.Object, _mockDiagnosticContext.Object);
            _personsDeleterService = new PersonsDeleterService(_mockPersonsDeleterRepository.Object, _mockPersonsGetterRepository.Object);
            _personsSorterService = new PersonsSorterService();
            _personsUpdaterService = new PersonsUpdaterService(_mockDiagnosticContext.Object, _mockPersonsUpdaterRepository.Object, _mockContactGroupsGetterRepository.Object);
        }

        #region AddPerson

        //Company
        [Fact]
        public async void AddPerson_WithCompany_ToBeSuccessful()
        {
            //Arrange
            Guid countryID = Guid.NewGuid();
            /*Company company = new Company()
            {
                CompanyId = 1,
                CompanyName = "Test",
            };*/

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "jojobart",
                Email = "some@gmail.com",
                Address = "New York, City",
                Gender = Gender.Get("Male"),
                DateOfBirth = new DateTime(2000, 12, 01),
                CountryId = countryID,
                CompanyId = 1,
            };            
            Person person = personAddRequest.ToPerson();

            /*_mockPersonsRepository.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);
            _mockCompaniesRepository.Setup(temp => temp.GetCompanyById(It.IsAny<int>())).ReturnsAsync(company);
            _mockCompaniesRepository.Setup(temp => temp.AddCompanyEmployee(It.IsAny<Person>(), It.IsAny<Company>())).ReturnsAsync(person);
            */
            //Act
            PersonResponse? personResponse = await _personsAdderService.AddPerson(personAddRequest);

            _testOutputHelper.WriteLine($"Person Response: {personResponse.PersonName}, {personResponse.CompanyId}");
            _testOutputHelper.WriteLine($"Person Request: {personAddRequest.PersonName}, {personAddRequest.CompanyId}");
            //Assert
            Assert.True(personAddRequest.CompanyId == personResponse.CompanyId);
        }
        //ContactGroup

            #endregion

            #region UpdatePerson

            #endregion

            #region DeletePerson

            #endregion
            #region GetPerson

            #endregion
    }
}
