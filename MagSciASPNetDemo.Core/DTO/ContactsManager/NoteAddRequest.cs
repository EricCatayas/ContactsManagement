using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.DTO.ContactsManager
{
    public class NoteAddRequest
    {
        [StringLength(50)]
        [DisplayName("Title")]
        public string? NoteTitle { get; set; } = string.Empty;
        [StringLength(500)]
        [DisplayName("Text")]
        public string NoteText { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public Guid PersonId { get; set; }
    }
}
