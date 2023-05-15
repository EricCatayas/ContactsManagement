using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.DTO.EventsManager
{
    public class EventUpdateRequest
    {
        [Required]
        public int EventId { get; set; }
        [Required]
        [StringLength(100)]
        public string? Title { get; set; }
        [StringLength(1000)]
        public string? Description { get; set; }
        [StringLength(30)]
        public string? Type { get; set; }
        [StringLength(30)]
        public string? ThemeColor { get; set; }
        [StringLength(30)]
        public string? Status { get; set; }
        public bool isCompleted { get; set; }
        [StartDateBeforeEndDate]
        [MinimumDateRange]
        public DateTime? StartDate { get; set; }
        [MinimumDateRange]
        public DateTime? EndDate { get; set; }
        public Guid? UserId { get; set; }

        public Event ToEvent()
        {
            return new Event()
            {
                EventId = EventId,
                Title = Title,
                Description = Description,
                Type = Type,
                Status = Status,
                ThemeColor = ThemeColor,
                isActive = !isCompleted,
                StartDate = StartDate,
                EndDate = EndDate,
            };
        }
    }
}
