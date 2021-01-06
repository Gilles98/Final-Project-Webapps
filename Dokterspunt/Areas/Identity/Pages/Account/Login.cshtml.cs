using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Dokterspunt.Areas.Identity.Data;
using Dokterspunt.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Dokterspunt.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<LoggedInUser> _userManager;
        private readonly SignInManager<LoggedInUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private DokterspuntContext _context;
        public LoginModel(SignInManager<LoggedInUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<LoggedInUser> userManager, DokterspuntContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                LoggedInUser user = await _userManager.FindByEmailAsync(Input.Email);

                //controle op user
                if (user != null)
                { 
                PasswordHasher<LoggedInUser> password = new PasswordHasher<LoggedInUser>();

                //gaat controle doen bij het gehashte passwoord in de database
                var passResult = password.VerifyHashedPassword(user, user.PasswordHash, Input.Password);
                    if (passResult == PasswordVerificationResult.Success)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password, Input.RememberMe, false);
                    
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                            //als login toch zou falen
                        ModelState.AddModelError(string.Empty, "Login is ongeldig.");
                        return Page();
                    }
                }
                else
                {
                        //als het wachtwoord incorrect is
                    ModelState.AddModelError(string.Empty, "Dit wachtwoord is incorrect");
                    return Page();
                }
               }
                else
                {
                        //als het e-mailadres niet is gevonden
                    ModelState.AddModelError(string.Empty, "Dit e-mailadres is niet gevonden");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
