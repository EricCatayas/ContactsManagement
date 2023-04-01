using ContactsManagement.Core.Enums.ContactsManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.Entities.ContactsManager
{
    public class ContactLog
    {
        [Key]
        public int LogId { get; set; }
        [Required]
        [StringLength(30)]
        public string? Type { get; set; }
        [StringLength(100)]
        public string? LogTitle { get; set; }
        [StringLength(500)]
        public string? Note { get; set; }
        public Guid? PersonId { get; set; }
        [ForeignKey(nameof(PersonId))]
        public virtual Person? PersonLog { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
