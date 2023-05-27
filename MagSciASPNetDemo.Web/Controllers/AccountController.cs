using ContactsManagement.Core.Domain.IdentityEntities;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.Enums.ContactsManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactGroupsServices;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactTagsServices;
using ContactsManagement.Core.ServiceContracts.EventsManager;
using ContactsManagement.Core.Services.ContactsManager.ContactGroups;
using ContactsManagement.Core.Services.EventsManager;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManagement.Web.Controllers
{
    [Route("Account")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IContactTagsSeederService _contactTagsSeederService;
        private readonly IContactGroupsSeederService _contactGroupsSeederService;
        private readonly IEventsSeederService _eventsSeederService;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IContactTagsSeederService contactTagsSeederService, IContactGroupsSeederService contactGroupsSeederService, IEventsSeederService eventsSeederService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _contactTagsSeederService = contactTagsSeederService;
            _contactGroupsSeederService = contactGroupsSeederService;
            _eventsSeederService = eventsSeederService;
        }
        [Route("Login")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO loginDTO, [FromForm] string? ReturnUrl, [FromForm] bool RememberMe = true)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage).ToList();
                return View(loginDTO);
            }
            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: RememberMe, lockoutOnFailure:false);
            
            //var userId = await _userManager.GetUserIdAsync(User);  <--

            if (result.Succeeded)
            {
                return ReturnUrl != null ? LocalRedirect(ReturnUrl) : RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
            {
                ModelState.AddModelError("Login", "Invalid email or password");
                return View(loginDTO);
            }
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("[action]")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("[action]")]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(V => V.Errors).Select(err => err.ErrorMessage).ToList();                
                return View(registerDTO);
            }
            ApplicationUser applicationUser = new()
            {
                PersonName = registerDTO.PersonName,
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.Phone,
                UserName = registerDTO.Email
            };


            IdentityResult result = await _userManager.CreateAsync(applicationUser, registerDTO.Password);
            if(result.Succeeded)
            {
                await _userManager.UpdateSecurityStampAsync(applicationUser);

                await _userManager.AddToRoleAsync(applicationUser, UserRoleOptions.User.ToString());

                await _signInManager.SignInAsync(applicationUser, isPersistent:true);

                await _contactGroupsSeederService.SeedUserContactGroups(applicationUser.Id);
                await _contactTagsSeederService.SeedUserContactTags(applicationUser.Id);
                await _eventsSeederService.SeedUserEvents(applicationUser.Id);

                return RedirectToAction(nameof(HomeController.Index),"Home");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError("Registration", error.Description);
                ViewBag.Errors = result.Errors.Select(error => error.Description).ToList();
                return View(registerDTO);
            }
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        // Enabling Google Login
        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string? returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
			//This method triggers the external authentication process and redirects the user to the Google login page.
			return Challenge(properties, provider);
        }
		[HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            if (remoteError != null)
            {
                // Handle error
                return RedirectToAction("Login");
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                // Handle error
                return RedirectToAction("Login");
            }
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {
                // User is successfully signed in
                if(returnUrl!= null)
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    return LocalRedirect("/");
                }
            }
            if (signInResult.IsLockedOut)
            {
                // Handle locked out user
                return RedirectToAction("Lockout");
            }
            else
            {
                // New user, prompt for additional registration information or handle as needed
                // Example: return RedirectToAction("AdditionalRegistration", new { email = info.Principal.FindFirstValue(ClaimTypes.Email) });
                return RedirectToAction("Registration");
            }
        }
        public async Task<IActionResult> ExternalLoginGoogleAsync(string googleTokenId)
        {
            GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings();
            settings.Audience = new List<string>() { "Get_Google_OAuth_ClientId" };
            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(googleTokenId, settings);

            ApplicationUser user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null) //create new user if not exsits
            {
                user = new ApplicationUser
                {
                    Email = payload.Email,
                    UserName = payload.Name
                };
        }
            return Ok();
        }

        public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
        {
            ApplicationUser? user =  await _userManager.FindByEmailAsync(email);


            return user == null ? Json(true) : Json(false); //The browser can only recieve the json result
        }
    }
}
