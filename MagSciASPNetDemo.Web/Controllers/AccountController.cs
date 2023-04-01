using ContactsManagement.Core.Domain.IdentityEntities;
using ContactsManagement.Core.DTO.ContactsManager;
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
        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager= signInManager;
            _userManager= userManager;

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
                await _signInManager.SignInAsync(applicationUser, isPersistent:true);

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
        public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
        {
            ApplicationUser? user =  await _userManager.FindByEmailAsync(email);


            return user == null ? Json(true) : Json(false); //The browser can only recieve the json result
        }
    }
}
