using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ContactsManagement.Core.DTO.ContactsManager.Contacts
{
    public class ContactLogAddRequest
    {
        [Required(ErrorMessage = "Communication Type is required")]
        [StringLength(30)]
        public string? Type { get; set; }
        [StringLength(100, ErrorMessage = "Title length must not exceed 100 characters")]
        [DisplayName("Title")]
        public string? LogTitle { get; set; } 
        [StringLength(2000, ErrorMessage = "Title length must not exceed 2000 characters")]
        public string? Note { get; set; }
        [Required]
        public Guid PersonId { get; set; }

        public ContactLog ToContactLog()
        {
            return new ContactLog()
            {
                Type = Type,
                LogTitle = LogTitle,
                Note = Note,
                PersonId = PersonId,
                DateCreated = DateTime.Now,
            };
        }
    }
}
