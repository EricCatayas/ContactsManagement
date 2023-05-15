using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices;
using ContactsManagement.Core.Services.ContactsManager.Persons;
using ContactsManagement.Infrastructure.DbContexts;
using EntityFrameworkCoreMock;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.UnitTests.Persons
{
    public class PersonsDeleterServiceTest
    {
        private readonly IPersonsDeleterService _personsDeleterService;

        private readonly Mock<IPersonsGetterRepository> _mockPersonsGetterRepository;
        private readonly Mock<IPersonsDeleterRepository> _mockPersonsDeleterRepository;
        public PersonsDeleterServiceTest()
        {
            _mockPersonsGetterRepository = new Mock<IPersonsGetterRepository>();
            _mockPersonsDeleterRepository = new Mock<IPersonsDeleterRepository>();

            _personsDeleterService = new PersonsDeleterService(_mockPersonsDeleterRepository.Object, _mockPersonsGetterRepository.Object);
        }
        #region DeletePerson
        [Fact]
        public async void DeletePerson_NonExistentPersonId()
        {
            Guid randomId = Guid.NewGuid();

            _mockPersonsGetterRepository.Setup(temp => temp.GetPersonById(It.IsAny<Guid>())).ReturnsAsync(null as Person);
            Assert.False(await _personsDeleterService.DeletePerson(randomId));
        }
        [Fact]
        public async void DeletePerson_PersonDeleted_ToBeSuccesful()
        {
            int countryID = 1000;
            Guid PersonId = Guid.NewGuid();   
            Person person = new Person()
            {
                Id = PersonId,
                Name = "jojobart1",
                Email = "some@gmail.com",
                Address = "New York, City",
                Gender = "Female",
                DateOfBirth = new DateTime(2000, 12, 01),
                CountryId = countryID,

                JobTitle = "Janitor",
                ContactNumber1 = "434-9090",
                ContactNumber2 = "434-9090",
            };

            _mockPersonsGetterRepository.Setup(temp => temp.GetPersonById(It.IsAny<Guid>()))
             .ReturnsAsync(person);
            _mockPersonsDeleterRepository.Setup(temp => temp.DeletePerson(It.IsAny<Person>())).ReturnsAsync(true);

            Assert.True(await _personsDeleterService.DeletePerson(person.Id));
        }
        #endregion
    }
}
