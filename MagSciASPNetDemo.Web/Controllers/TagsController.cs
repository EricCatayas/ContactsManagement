using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactTagsServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManagement.Web.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    public class TagsController : Controller
    {
        private readonly IContactTagsAdderService _contactTagsAdderService;
        private readonly IContactTagsUpdaterService _contactTagsUpdaterService;
        private readonly IContactTagsDeleterService _contactTagsDeleterService;

        public TagsController(IContactTagsAdderService contactTagsAdderService, IContactTagsUpdaterService contactTagsUpdaterService, IContactTagsDeleterService contactTagsDeleterService)
        {
            _contactTagsAdderService = contactTagsAdderService;
            _contactTagsUpdaterService = contactTagsUpdaterService;
            _contactTagsDeleterService = contactTagsDeleterService;
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<JsonResult> RemoveContactTagFromPerson([FromQuery]Guid personId)
        {
            bool isRemoved = await _contactTagsUpdaterService.RemoveContactTagFromPerson(personId);
            if(isRemoved)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create(ContactTagAddRequest contactTagAddRequest)
        {
            if (ModelState.IsValid)
            {
                _ = await _contactTagsAdderService.AddContactTag(contactTagAddRequest);
                return RedirectToAction("Index", "Persons");
            }
            else
            {
                List<string> validationErrors = ModelState.Values.SelectMany(V => V.Errors).Select(err => err.ErrorMessage).ToList();  
                return RedirectToAction("Index", "ContactGroups", new {error = validationErrors});
            }           
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Delete(int TagId)
        {
            bool isDeleted = await _contactTagsDeleterService.DeleteContactTag(TagId);
            if (isDeleted)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }
}
