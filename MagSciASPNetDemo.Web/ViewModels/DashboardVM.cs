using ContactsManagement.Core.DTO.EventsManager;

namespace ContactsManagement.Web.ViewModels
{
    public class DashboardVM
    {
        public List<EventResponse> ActiveEvents { get; set; }
        public List<EventResponse> CompletedEvents { get; set; }
        public DashboardVM() 
        {
            ActiveEvents = new List<EventResponse>();
            CompletedEvents = new List<EventResponse>();
        }

    }
}
