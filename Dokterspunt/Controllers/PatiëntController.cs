using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dokterspunt.Areas.Identity.Data;
using Dokterspunt.Data;
using Dokterspunt.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dokterspunt.Controllers
{
    [Authorize(Roles = "Patiënt")]
    public class PatiëntController : Controller
    {
        private readonly DokterspuntContext _context;
        private readonly UserManager<LoggedInUser> _userManager;
        private readonly SignInManager<LoggedInUser> _signInManager;

        public PatiëntController(DokterspuntContext context, UserManager<LoggedInUser> userManager, SignInManager<LoggedInUser>signInManager)
        {
            this._context = context;

            this._signInManager = signInManager;
            this._userManager = userManager;
        }
        // GET: Patiënt
        public async Task<ActionResult> Index(int? id)
        {
            if (id > 0 && id != null)
            {
                UpdatePatiëntGegevensViewModel viewModel = new UpdatePatiëntGegevensViewModel()
                {
                    Patiënt = await _context.Patiënten.Where(x => x.PatiëntID == id).SingleOrDefaultAsync(),
                };
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }
        }


        // CREATE GEBEURT BIJ REGISTREREN!



        //gaat de edit en de delete posts regelen plus token
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProcessForm(UpdatePatiëntGegevensViewModel viewModel, string bewaren, string deleten)
        {
            if (!string.IsNullOrEmpty(bewaren))
            {
                var edit = await Edit(viewModel);
                if (edit != null)
                {
                    return edit;
                }
            }
            if (!string.IsNullOrEmpty(deleten))
            {
                var delete = await Delete(viewModel);
                if (delete != null)
                {
                    return delete;
                }
            }
            return RedirectToAction("Index", "Error");
        }


        public async Task<ActionResult> Edit(UpdatePatiëntGegevensViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //user ophalen
                    LoggedInUser user = await _userManager.FindByIdAsync(viewModel.Patiënt.UserID);
                   
                    //kijken of er gevraagd is een nieuw wachtwoord op te geven
                    if (viewModel.Wachtwoord != null)
                    {
                        //wachtwoord herinstellen
                        var nieuwWachtwoord = new PasswordHasher<LoggedInUser>().HashPassword(user, viewModel.Wachtwoord);
                        user.PasswordHash = nieuwWachtwoord; 
                    }
                    user.Patient = viewModel.Patiënt;
                    if (viewModel.Email != null)
                    {

                        user.Email = viewModel.Email;
                        user.NormalizedEmail = user.Email.ToUpper();
                        user.UserName = user.Email;
                        user.NormalizedUserName = user.UserName.ToUpper();
                    }
                        var ok = await _userManager.UpdateAsync(user);
                        if (ok.Succeeded)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        public async Task<ActionResult> Delete(UpdatePatiëntGegevensViewModel viewModel)
        {
            try
            {
                LoggedInUser user = await _userManager.FindByIdAsync(viewModel.Patiënt.UserID);
                if (user != null)
                {
                    _context.Patiënten.Remove(viewModel.Patiënt);
                    await _context.SaveChangesAsync();
                    await _userManager.DeleteAsync(user);
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
