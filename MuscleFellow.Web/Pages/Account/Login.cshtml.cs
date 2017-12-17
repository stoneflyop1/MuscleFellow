using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using MuscleFellow.Models.Domain;
using MuscleFellow.Web.Services;
using Microsoft.Extensions.Logging;
using MuscleFellow.Web.Models;

namespace MuscleFellow.Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginViewModel LoginUser { get; set; }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICartItemService _cartService;
        private readonly ILogger _logger;

        public LoginModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ICartItemService cartService, ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cartService = cartService;
            _logger = loggerFactory.CreateLogger<RegisterModel>();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToPage("/Index");// RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion

        public void OnGet()
        {
            ViewData["ReturnUrl"] = Request.Query["returnurl"];
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(
                    LoginUser.Email, LoginUser.Password, LoginUser.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    // Update Shopping cart in anonymous status.
                    string sessionID = HttpContext.Session.Id;
                    int count = await _cartService.UpdateAnonymousCartItem(sessionID, LoginUser.Email);
                    _logger.LogInformation(1, "User logged in.");
                    return RedirectToLocal(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return Page();//RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");
                    return Page(); //View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page(); //View(model);
                }
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}