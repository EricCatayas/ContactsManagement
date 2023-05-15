using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.DTO.EventsManager
{
    public class EventResponse
    {
        public int EventId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? ThemeColor { get; set; }
        public string? Status { get; set; }
        public bool isActive { get; set; }        
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
    public static class EventExtentions
    {
        public static EventResponse ToEventResponse(this Event @event)
        {
            return new EventResponse()  
            {
                EventId = @event.EventId,
                Title = @event.Title,
                Description = @event.Description,
                Type = @event.Type,
                ThemeColor = @event.ThemeColor,
                Status = @event.Status,
                isActive = @event.isActive,
                StartDate = @event.StartDate,
                EndDate = @event.EndDate,
                LastUpdatedDate = @event.LastUpdatedDate,
            };
        }
        public static EventUpdateRequest ToEventUpdateRequest(this EventResponse @event)
        {
            return new EventUpdateRequest()
            {
                EventId = @event.EventId,
                Title = @event.Title,
                Description = @event.Description,
                Type = @event.Type,
                ThemeColor = @event.ThemeColor,
                Status = @event.Status,
                isCompleted = !@event.isActive,
                StartDate = @event.StartDate,
                EndDate = @event.EndDate,
            };
        }
    }
}
