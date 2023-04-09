using ContactsManagement.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.Entities.ContactsManager
{
    public class Event
    {
        [Key]
        public int EventId { get; set; } 
        [Required]
        [StringLength(100)]
        public string? Title { get; set; }
        [StringLength(1000)]
        public string? Description { get; set; }
        [StringLength(30)]
        public string? Type { get; set; }
        [StringLength(30)]
        public string? Status { get; set; }
        [StringLength(30)]
        public string? ThemeColor { get; set; }
        public bool isActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public Guid? UserId { get; set; }
    }
}
