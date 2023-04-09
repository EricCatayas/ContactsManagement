using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.Entities.ContactsManager
{
    /// <summary>
    /// Used to label or categorize contacts. This entity can have fields such as tag name and description. This entity can have a many-to-many relationship with the Person entity
    /// </summary>
    public class ContactTag
    {
        [Key]
        public int TagId { get; set; }
        [Required]
        [StringLength(50)]
        public string? TagName { get; set; }
        [StringLength(30)]
        public string? TagColor { get; set; }
        public Guid? UserId { get; set; }
    }
}
