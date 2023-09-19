
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.Exceptions;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactGroupsServices;
using ContactsManagement.Core.Services.ContactsManager.ContactGroups;
using Moq;

namespace ContactsManagement.UnitTests.ContactGroups
{
    public class ContactGroupsUpdaterServiceTest
    {
        private readonly Mock<ISignedInUserService> _mockSignedInUserService;
        private readonly Mock<IContactGroupsUpdaterRepository> _mockContactGroupsUpdaterRepository = new();
        private readonly Mock<IPersonsGetterRepository> _mockPersonsGetterRepository = new();
        private readonly IContactGroupsUpdaterService _contactGroupsUpdaterService;
        public ContactGroupsUpdaterServiceTest()
        {
            _mockSignedInUserService = new Mock<ISignedInUserService>();
            _contactGroupsUpdaterService = new ContactGroupsUpdaterService(_mockContactGroupsUpdaterRepository.Object, _mockPersonsGetterRepository.Object, _mockSignedInUserService.Object);
        }
        [Fact]
        public async Task AddContactGroup_NonSignedInRequest_ToThrowAccessDeniedException()
        {
            Guid? nullUserId = null;

            ContactGroupUpdateRequest contactGroup_ToUpdate = new()
            {
                GroupName = "Sample Test",
                Description = "Sample Test",
                Persons = new List<Guid> { }
            };

            _mockSignedInUserService.Setup(temp => temp.GetSignedInUserId()).Returns(nullUserId);

            await Assert.ThrowsAsync<AccessDeniedException>(async () =>
            {
                await _contactGroupsUpdaterService.UpdateContactGroup(contactGroup_ToUpdate);
            });
        }
    }
}
