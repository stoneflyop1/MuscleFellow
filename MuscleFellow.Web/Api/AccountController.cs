using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MuscleFellow.ApiModels;
using MuscleFellow.Models.Domain;
using MuscleFellow.Web.Jwt;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MuscleFellow.Web.Api
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] LoginAPIModel registerModel)
        {
            var user = new ApplicationUser { UserName = registerModel.UserID, Email = registerModel.UserID };
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: true);
                _logger.LogInformation(3, "User created a new account with password.");
                return Ok();
            }

            // If we got this far, something failed.
            return Unauthorized();
        }
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginAPIModel loginModel)
        {
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(
                    loginModel.UserID, loginModel.Password, true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation(1, "User logged in.");

                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup.SecretKey));
                var options = new TokenProviderOptions
                {
                    Audience = "MuscleFellowAudience",
                    Issuer = "MuscleFellow",
                    SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                };
                TokenProvider tpm = new TokenProvider(options);
                TokenEntity token = await tpm.GenerateToken(HttpContext, loginModel.UserID, loginModel.Password);
                if (null != token)
                    return new JsonResult(token);
                else
                    return NotFound();
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning(2, "User account locked out.");
                return Ok("Lockout");
            }
            else
            {
                _logger.LogWarning(2, "Invalid login attempt.");
                return Ok("Invalid login attempt.");
            }
        }
        // POST: /Account/LogOff
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return Ok();
        }

    }
}
