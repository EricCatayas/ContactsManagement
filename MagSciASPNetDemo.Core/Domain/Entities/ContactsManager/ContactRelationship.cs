using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.Entities.ContactsManager
{
    /// <summary>
    /// Redundant
    /// </summary>
    public class ContactRelationship
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string? Type { get; set; }
        public virtual ICollection<Person> Persons { get; set; }

    }
}
