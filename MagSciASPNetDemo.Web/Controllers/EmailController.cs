using Azure;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.Exceptions;
using ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices;
using ContactsManagement.Core.ServiceContracts.EmailServices;
using Google.Apis.Auth.OAuth2;
using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authentication;
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
        [Route("[action]")]
		public async Task<IActionResult> ImportContacts()
		{
			var accessToken = await HttpContext.GetTokenAsync("access_token");

			var credential = GoogleCredential.FromAccessToken(accessToken);
			var service = new PeopleServiceService(new BaseClientService.Initializer
			{
				HttpClientInitializer = credential,
				ApplicationName = "ContactsManagement-GoogleAuth"
			});

			var contactsRequest = service.People.Connections.List("people/me");
			contactsRequest.RequestMaskIncludeField = "person.names,person.emailAddresses";
			ListConnectionsResponse? contactsResponse = await contactsRequest.ExecuteAsync();

            if(contactsResponse == null)
            {
                return StatusCode(500);
            }
			// Process the contacts data from contactsResponse
			IList<Person> contactsData = contactsResponse.Connections;
            if(contactsData.Count > 0)
            {
                List<PersonAddRequest> personsToAdd = new List<PersonAddRequest>();
				foreach (Person person in contactsData)
				{
                    var personToAdd = new PersonAddRequest()
                    {
                        PersonName = person.Names.First().ToString(),
                        Email = person.EmailAddresses.First().ToString(),
                        Address = person.Addresses.First().ToString(),
                        // DateOfBirth = person.Birthdays.First(),
                        ContactNumber1 = person.PhoneNumbers[0].Value.ToString(),
                        ContactNumber2 = person.PhoneNumbers[1].Value.ToString(),

                    };
                    personsToAdd.Add(personToAdd);
				}
			}            

			return Ok();
		}
	}
}
