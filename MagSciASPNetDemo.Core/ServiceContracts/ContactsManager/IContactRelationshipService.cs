using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager
{
    public interface IContactRelationshipService
    {
        public Task<string?> AddContactRelationship(Guid PersonId, int RelationshipId);
        public Task<bool?> RemoveContactRelationship(Guid PersonId, int RelationshipId);
        public Task<ContactRelationshipDTO> GetContactRelationshipDTO(int RelationshipId);
    }
}
