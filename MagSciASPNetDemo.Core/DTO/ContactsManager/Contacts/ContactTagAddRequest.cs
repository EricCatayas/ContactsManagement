using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.DTO.ContactsManager.Contacts
{
    public class ContactTagAddRequest
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Tag name must not exceed 50 characters")]
        public string? TagName { get; set; }
        [StringLength(maximumLength: 30, ErrorMessage = "Color name is invalid")]
        public string? TagColor { get; set; }

        public ContactTag ToContactTag()
        {
            return new ContactTag()
            {
                //TagId
                TagName = TagName,
                TagColor = TagColor,
            };
        }
    }
}
