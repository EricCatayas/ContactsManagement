using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactTagsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.ContactsManager.ContactTags
{
    public class ContactTagsUpdaterService : IContactTagsUpdaterService
    {
        private readonly IContactTagsUpdaterRepository _contactTagsUpdaterRepository;
        private readonly IPersonsGetterRepository _personsGetterRepository;

        public ContactTagsUpdaterService(IContactTagsUpdaterRepository contactTagsUpdaterRepository, IPersonsGetterRepository personsGetterRepository)
        {
            _contactTagsUpdaterRepository = contactTagsUpdaterRepository;
            _personsGetterRepository = personsGetterRepository;
        }
        public async Task<bool> RemoveContactTagFromPerson(Guid personId)
        {
            Person? person = await _personsGetterRepository.GetPersonById(personId);
            if (person == null) return false;

            person.TagId = null;
            return await _contactTagsUpdaterRepository.UpdatePersonContactTag(person);
        }
    }
}
