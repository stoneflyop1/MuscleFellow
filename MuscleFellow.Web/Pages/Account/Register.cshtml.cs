﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using MuscleFellow.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MuscleFellow.Web.Services;
using MuscleFellow.Web.Models;

namespace MuscleFellow.Web.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICartItemService _cartService;
        private readonly ILogger _logger;

        public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ICartItemService cartService, ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cartService = cartService;
            _logger = loggerFactory.CreateLogger<RegisterModel>();
        }
        [BindProperty]
        public RegisterViewModel RegisterUser { get; set; }

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

        public void OnGet(string returnUrl)
        {            
            ViewData["ReturnUrl"] = returnUrl;
        }
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = RegisterUser.Email, Email = RegisterUser.Email };
                var result = await _userManager.CreateAsync(user, RegisterUser.Password);
                if (result.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                    //    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Update Shopping cart in anonymous status.
                    string sessionID = HttpContext.Session.Id;
                    int count = await _cartService.UpdateAnonymousCartItem(sessionID, RegisterUser.Email);
                    _logger.LogInformation(3, "User created a new account with password.");
                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return Page();  //RedirectToPage();
        }
    }
}