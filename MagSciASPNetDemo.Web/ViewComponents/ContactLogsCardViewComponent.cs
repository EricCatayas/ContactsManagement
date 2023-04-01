using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactLogsServices;
using ContactsManagement.Core.Services.ContactsManager.ContactLogs;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManagement.Web.ViewComponents
{
    public class ContactLogsCardViewComponent : ViewComponent
    {
        private readonly IContactLogsGetterService _contactLogsGetterService;

        public ContactLogsCardViewComponent(IContactLogsGetterService contactLogsGetterService)
        {
            _contactLogsGetterService = contactLogsGetterService;
        }
        public async Task<IViewComponentResult> InvokeAsync(Guid personId)    // gets called auto -- def: Views/Shared/Components/<Grid>/Default.cshtml
        {
            List<ContactLogResponse>? contactLogs = await _contactLogsGetterService.GetContactLogsFromPerson(personId);
            return View("ContactLogsCard", contactLogs);  // a partial view -- supply name of view
        }
    }
}
