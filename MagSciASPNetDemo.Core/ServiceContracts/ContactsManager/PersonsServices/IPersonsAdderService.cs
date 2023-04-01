using ContactsManagement.Core.DTO.ContactsManager;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices
{
    public interface IPersonsAdderService
    {
        Task<PersonResponse?> AddPerson(PersonAddRequest? personAddRequest);
    }
}
