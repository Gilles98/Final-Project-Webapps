using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Dokterspunt.Areas.Identity.Data;
using Dokterspunt.Data;
using Dokterspunt.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dokterspunt.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<LoggedInUser> _signInManager;
        private readonly UserManager<LoggedInUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly DokterspuntContext _context;
        public RegisterModel(
            UserManager<LoggedInUser> userManager,
            SignInManager<LoggedInUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, DokterspuntContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Het veld Email is verplicht")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Het veld wachtwoord is vereist")]
            [StringLength(100, ErrorMessage = "Het wachtwoord moet minstens {2} karakters lang zijn.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Wachtwoord")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Voornaam is vereist")]
            [DataType(DataType.Text)]
            public string Voornaam { get; set; }

            [Required(ErrorMessage = "Achternaam is vereist")]
            [DataType(DataType.Text)]
            public string Achternaam { get; set; }

            [Required(ErrorMessage = "Gemeente is vereist")]
            [DataType(DataType.Text)]
            public string Gemeente { get; set; }

            [Required(ErrorMessage = "Straat is vereist")]
            [DataType(DataType.Text)]
            public string Straat { get; set; }

            [Required(ErrorMessage = "Huisnummer is vereist")]
            [DataType(DataType.Text)]
            public string Huisnummer { get; set; }


            [Required(ErrorMessage = "Herhaal wachtwoord is vereist")]
            [DataType(DataType.Password)]
            [Display(Name = "Bevestig wachtwoord")]
            [Compare("Password", ErrorMessage = "De wachtwoorden komen niet overeen")]
            public string ConfirmPassword { get; set; }

            [Required]
            [DataType(DataType.PostalCode)]
            public int Postcode { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {

                LoggedInUser user = await _userManager.FindByEmailAsync(Input.Email);
                if (user != null)
                {
                    ModelState.AddModelError(string.Empty, "Er is al een gebruiker geregistreerd met dit email adress");
                    return Page();
                }
                else
                {
                user = new LoggedInUser();
                user.UserName = Input.Email;
                user.NormalizedUserName = Input.Email.ToUpper();
                user.Email = Input.Email;
                user.NormalizedEmail = user.Email.ToUpper();
                user.EmailConfirmed = true;
                user.Patient = new Patiënt() { Voornaam = Input.Voornaam, Achternaam = Input.Achternaam, Gemeente = Input.Gemeente, Postcode = Input.Postcode, HuisNr = Input.Huisnummer, LoggedUser = user, UserID = user.Id, Straat = Input.Straat};;
                user.LockoutEnabled = false;
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {

                    IdentityRole role = _context.Roles.FirstOrDefault(x => x.Name == "Patiënt");
                    DbSet<IdentityUserRole<string>> roles = _context.UserRoles;
                    if (!roles.Any(us => us.UserId == user.Id && us.RoleId == role.Id))
                    {
                        roles.Add(new IdentityUserRole<string>() { UserId = user.Id, RoleId = role.Id });
                    }
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
               }
              }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
