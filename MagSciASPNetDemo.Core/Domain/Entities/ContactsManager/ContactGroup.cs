using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.Entities.ContactsManager
{
    public class ContactGroup
    {
        [Key]
        public int GroupId { get; set; } 
        [Required]
        [StringLength(100)]
        public string? GroupName { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        public virtual ICollection<Person>? Persons { get; set; }
    }
}
