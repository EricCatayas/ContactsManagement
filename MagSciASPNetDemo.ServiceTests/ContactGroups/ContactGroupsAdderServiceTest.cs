
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.Exceptions;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactGroupsServices;
using ContactsManagement.Core.Services.ContactsManager.ContactGroups;
using ContactsManagement.Infrastructure.Repositories.ContactsManager.ContactGroups;
using Moq;

namespace ContactsManagement.UnitTests.ContactGroups
{
    public class ContactGroupsAdderServiceTest
    {
        private readonly Mock<ISignedInUserService> _mockSignedInUserService;
        private readonly Mock<IContactGroupsAdderRepository> _mockContactGroupsAdderRepository = new();
        private readonly IContactGroupsAdderService _contactGroupsAdderService; 
        public ContactGroupsAdderServiceTest()
        {
            _mockSignedInUserService = new Mock<ISignedInUserService>();
            _contactGroupsAdderService = new ContactGroupsAdderService(_mockContactGroupsAdderRepository.Object, _mockSignedInUserService.Object);
        }
        [Fact]
        public async Task AddContactGroup_NonSignedInRequest_ToThrowAccessDeniedException()
        {
            Guid? nullUserId = null;

            ContactGroupAddRequest contactGroup_ToAdd = new()
            {
                GroupName = "Sample Test",
                Description = "Sample Test",
                Persons = new List<Guid> { }
            };

            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(nullUserId);

            await Assert.ThrowsAsync<AccessDeniedException>(async () =>
            {
                await _contactGroupsAdderService.AddContactGroup(contactGroup_ToAdd);
            });
        }
    }
}
