using Azure;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.Exceptions;
using ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices;
using ContactsManagement.Core.ServiceContracts.EmailServices;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManagement.Web.Controllers
{
    [NonController]
    public class EmailController : Controller
    {
        private readonly IPersonsGetterService _personsGetterService;
        private readonly IEmailService _emailService;

        public EmailController(IPersonsGetterService personsGetterService, IEmailService emailService)
        {
            _personsGetterService = personsGetterService;
            _emailService = emailService;
        }
		[Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Create(Guid personId)
        {
			var recipient = await _personsGetterService.GetPersonById(personId);
            if(recipient == null)
            {
                throw new InvalidPersonIDException();
            }
            ViewBag.EmailAddress = recipient.Email;
            return View();
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Create(string recipient, string subject, string body)
        {
            bool isEmailSent = await _emailService.SendEmail(recipient, subject, body);
            if (!isEmailSent)
            {
                //TODO
            }
            return View();
        }       
	}
}
