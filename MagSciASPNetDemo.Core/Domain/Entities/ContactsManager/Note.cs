using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.Entities.ContactsManager
{
    /// <summary>
    /// Redundant for Person i.e. ContactLog
    /// </summary>
    public class Note
    {
        [Key]
        public int NoteId { get; set; }
        [StringLength(50)]
        public string? NoteTitle { get; set; }
        [StringLength(500)]
        public string NoteText { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
    }
}
