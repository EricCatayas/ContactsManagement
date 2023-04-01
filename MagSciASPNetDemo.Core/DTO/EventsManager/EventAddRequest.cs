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
    public class EventAddRequest
    {
        [Required]
        [StringLength(100)]
        public string? Title { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        [StringLength(30)]
        public string? Type { get; set; }
        [StringLength(30)]
        public string? ThemeColor { get; set; }
        public bool isActive { get; set; }  = true;        
        [MinimumDateRange]
        [StartDateBeforeEndDate]
        public DateTime? StartDate { get; set; }
        [MinimumDateRange]
        public DateTime? EndDate { get; set; }

        public Event ToEvent()
        {
            return new()
            {
                Title = Title,
                Description = Description,
                Type = Type,
                ThemeColor = ThemeColor,
                isActive = isActive,
                StartDate = StartDate,
                EndDate = EndDate,
                //LastUpdatedDate = DateTime.Now
            };
        }
    }
}
