using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.Identity.Data;
using MVC.Identity.ViewModels.Auth;

namespace MVC.Identity.Controllers
{
    [Authorize]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// check if email is already in database
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailTaken(string email)
        {
            var userEmail = await _userManager.FindByEmailAsync(email);
            if (userEmail != null)
            {
                return Json($"Email {email} is already taken");
            }
            else
            {
                return Json(true);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegistrationVM registrationVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new()
                {
                    FName = registrationVM.FName,
                    LName = registrationVM.LName,
                    Email = registrationVM.Email,
                };

                user.UserName = registrationVM.Email;
                var result = await _userManager.CreateAsync(user, registrationVM.Password);

                if (result.Succeeded)
                {
                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Super Admin"))
                    {
                        //user.CreatedBy = User.
                        return RedirectToAction("AllUsers", "Administration");
                    }

                    TempData["Successful"] = " Account created successfully";
                    return RedirectToAction("Login");
                }


                foreach (var error in result.Errors)
                {

                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(registrationVM);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM loginVM, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, false, false);
                if (user.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid credentials, please try again");
                }
            }
            return View(loginVM);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
