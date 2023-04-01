using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.DTO.ContactsManager.Contacts
{
    public class ContactTagDTO
    {
        public int TagId { get; set; }
        public string? TagName { get; set; }
        public string? TagColor { get; set; }
    }
    public static class ContactTagResponseExtentions
    {
        public static ContactTagDTO ToContactTagResponse(this ContactTag contactTag)
        {
            return new ContactTagDTO()
            {
                TagId = contactTag.TagId,
                TagName = contactTag.TagName,
                TagColor = contactTag.TagColor
            };
        }
        public static ContactTag ToContactTag(this ContactTagDTO contactTag)
        {
            return new ContactTag()
            {
                TagId = contactTag.TagId,
                TagName = contactTag.TagName,
                TagColor = contactTag.TagColor
            };
        }
    }
}
