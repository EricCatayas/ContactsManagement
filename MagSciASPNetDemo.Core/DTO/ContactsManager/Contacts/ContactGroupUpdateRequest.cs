using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.DTO.ContactsManager.Contacts
{
    public class ContactGroupUpdateRequest
    {
        [Required]
        public int GroupId { get; set; }
        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Tag name must not exceed 100 characters")]
        public string GroupName { get; set; }
        [StringLength(maximumLength: 500, ErrorMessage = "Tag name must not exceed 500 characters")]
        public string? Description { get; set; }
        public List<Guid>? Persons { get; set; }

        public ContactGroup ToContactGroup()
        {
            return new ContactGroup()
            {
                GroupId = GroupId,
                GroupName = GroupName,
                Description = Description,
                // Persons
            };
        }
    }
}
