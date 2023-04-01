using Serilog;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.Domain.Entities.ContactsManager;

namespace ContactsManagement.Core.Services.ContactsManager
{
    public class PersonsDeleterService : IPersonsDeleterService
    {
        // R: Logger in Repository
        private readonly IPersonsDeleterRepository _personsDeleterRepository;
        private readonly IPersonsGetterRepository _personsGetterRepository;
        private readonly IContactLogsDeleterRepository _contactLogsDeleterRepository;

        public PersonsDeleterService(IPersonsDeleterRepository personsRepository, IPersonsGetterRepository personsGetterRepository, IContactLogsDeleterRepository contactLogsDeleterRepository)
        {
            _personsDeleterRepository = personsRepository;
            _personsGetterRepository = personsGetterRepository;
            _contactLogsDeleterRepository = contactLogsDeleterRepository;
        }
        public async Task<bool> DeletePerson(Guid personId)
        {
            Person? person = await _personsGetterRepository.GetPersonById(personId);

            if (person == null) return false;

            bool isDeleted = await _personsDeleterRepository.DeletePerson(person);
            if( isDeleted && person.ContactGroups!= null)
            {
                await _contactLogsDeleterRepository.DeleteContactLogsFromPerson(person);
                return true;
                
            }
            else
            {
                return false;
            }
        }
    }
}
