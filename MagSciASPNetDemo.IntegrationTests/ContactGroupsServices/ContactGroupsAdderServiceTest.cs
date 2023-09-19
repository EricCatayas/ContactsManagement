using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.Exceptions;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactGroupsServices;
using ContactsManagement.Core.Services.ContactsManager.ContactGroups;
using ContactsManagement.Infrastructure.DbContexts;
using ContactsManagement.Infrastructure.Repositories.ContactsManager.ContactGroups;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContactsManagement.IntegrationTests.ContactGroupsServices
{
    public class ContactGroupsAdderServiceTest
    {
        private readonly Mock<ISignedInUserService> _mockSignedInUserService;
        private readonly Mock<IPersonsGetterRepository> _mockPersonsGetterRepository = new();
        public ContactGroupsAdderServiceTest()
        {
            _mockSignedInUserService = new Mock<ISignedInUserService>();
        }        
        [Fact]
        public async void AddContactGroup_WithPersons_ToBeSuccessful()
        {
            Guid UserId = Guid.NewGuid();
            Person person1 = new()
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
            ContactGroupAddRequest contactGroup_ToAdd = new()
            {
                GroupName = "Sample Test",
                Description = "Sample Test",
                Persons = new List<Guid> { person1.Id, person2.Id }
            };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();

                context.Persons.Add(person1);
                context.Persons.Add(person2);
                context.SaveChanges();

                IContactGroupsAdderRepository contactGroupsAdderRepository = new ContactGroupsAdderRepository(context);
                IContactGroupsAdderService contactGroupsAdderService = new ContactGroupsAdderService(contactGroupsAdderRepository, _mockPersonsGetterRepository.Object, _mockSignedInUserService.Object);

                _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(UserId);
                _mockPersonsGetterRepository.Setup(temp => temp.GetPersonsById(It.IsAny<List<Guid>>())).ReturnsAsync(new List<Person>() { person1, person2 });
                //Act
                ContactGroupResponse contactGroup_FromAddContactGroup = await contactGroupsAdderService.AddContactGroup(contactGroup_ToAdd);

                ContactGroup? contactGroup = await context.ContactGroups.Include(temp => temp.Persons).FirstOrDefaultAsync(temp => temp.GroupId == contactGroup_FromAddContactGroup.GroupId);

                Assert.NotNull(contactGroup);
                Assert.True(contactGroup?.Persons.Any(temp => temp.Id == person1.Id));
                Assert.True(contactGroup?.Persons.Any(temp => temp.Id == person2.Id));
            }
        }
    }
}
