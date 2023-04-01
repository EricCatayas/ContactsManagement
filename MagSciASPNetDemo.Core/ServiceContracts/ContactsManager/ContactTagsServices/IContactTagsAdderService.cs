using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.ContactTagsServices
{
    public interface IContactTagsAdderService
    {
        Task<ContactTagDTO> AddContactTag(ContactTagAddRequest contactTag);
    }
}
