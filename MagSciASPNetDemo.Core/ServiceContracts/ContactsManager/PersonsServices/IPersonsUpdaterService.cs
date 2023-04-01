using ContactsManagement.Core.DTO.ContactsManager;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices
{
    public interface IPersonsUpdaterService
    {
        /// <summary>
        /// Updates the person with the corresponding Id to the person argument
        /// </summary>
        /// <param name="personUpdateRequest">The person that will overwrite the current person</param>
        /// <returns>returns the updated person</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);
    }
}
