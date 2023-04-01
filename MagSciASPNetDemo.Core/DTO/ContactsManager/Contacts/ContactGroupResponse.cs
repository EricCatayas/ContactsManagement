using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.DTO.ContactsManager.Contacts
{
    public class ContactGroupResponse
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string? Description { get; set; } = string.Empty;
        public List<Guid>? Persons { get; set; }
    }
    public static class ContactGroupResponseExtentions
    {
        public static ContactGroupResponse ToContactGroupResponse(this ContactGroup contactGroup)
        {
            return new ContactGroupResponse()
            {
                GroupId = contactGroup.GroupId,
                GroupName = contactGroup.GroupName,
                Description = contactGroup.Description,
                Persons = contactGroup.Persons.Select(person => person.Id).ToList(),
            };
        }
        public static ContactGroupUpdateRequest ToContactGroupUpdateRequest(this ContactGroupResponse contactGroup)
        {
            return new ContactGroupUpdateRequest()
            {
                GroupId = contactGroup.GroupId,
                GroupName = contactGroup.GroupName,
                Description = contactGroup.Description,
                Persons = contactGroup.Persons
            };
        }
    }
}
