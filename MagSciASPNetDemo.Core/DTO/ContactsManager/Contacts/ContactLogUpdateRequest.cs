using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.DTO.ContactsManager.Contacts
{
    public class ContactLogUpdateRequest
    {
        [Key]
        [Required]
        public int LogId { get; set; }
        [Required]
        [StringLength(30)]
        public string? Type { get; set; }
        [StringLength(100, ErrorMessage = "Title length must not exceed 100 characters")]
        [DisplayName("Title")]
        public string? LogTitle { get; set; }
        [StringLength(2000, ErrorMessage = "Note length must not exceed 2000 characters")]
        public string? Note { get; set; } 
        public Guid? PersonId { get; set; }
        public DateTime? DateCreated { get; set; }
        public Guid? UserId { get; set; }
        public ContactLog ToContactLog()
        {
            return new ContactLog()
            {
                LogId = LogId,
                Type = Type,
                LogTitle = LogTitle,
                Note = Note,
                PersonId = PersonId,
                DateCreated = DateCreated,
                UserId = UserId,
            };
        }
    }
}
