using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dokterspunt.Areas.Identity.Data;
using Dokterspunt.Data;
using Dokterspunt.Models;
using Dokterspunt.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Dokterspunt.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DokterController : Controller
    {
        // GET: DokterController
        private readonly DokterspuntContext _context;
        private readonly UserManager<LoggedInUser> _userManager;
        public DokterController(DokterspuntContext context, UserManager<LoggedInUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        public async Task<ActionResult> Index()
        {
            ///de admin mag niet gewijzigd worden!
            ///omdat dit toch enkel mogelijk is voor de admin
            LoggedInUser gebruiker = await _userManager.FindByNameAsync(User.Identity.Name);
            IndexDokterViewModel viewModel = new IndexDokterViewModel()
            {
                Create = new CreateDokterModel()
                {
                    Praktijken = new SelectList(_context.Praktijken, "PraktijkID", "VolledigAdress"),
                    Specialiteiten = new SelectList(_context.Specialisaties, "SpecialisatieID", "Omschrijving")
                }
                ,
                Update = new UpdateDokterModel()
                {
                    Praktijken = new SelectList(_context.Praktijken, "PraktijkID", "VolledigAdress"),
                    Specialiteiten = new SelectList(_context.Specialisaties, "SpecialisatieID", "Omschrijving"),
                    Dokters = new SelectList(_context.Dokters.Where(x => x.UserID != gebruiker.Id), "DokterID", "VolledigeGegevens")
                }



            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<JsonResult> ZoekDokter(int id)
        {
            //gaat json object returnen
            Dokter result = await _context.Dokters.Where(x => x.DokterID == id).Include(x => x.Praktijk).Include(x => x.Specialisatie).Include(x => x.LoggedUser).SingleOrDefaultAsync();
            return Json(result);
        }


        // POST: DokterController/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        //op 1 form staan 2 mogelijke acties met delete en edit. deze methode gaat dit regelen
        public async Task<IActionResult> ProcessForm(IndexDokterViewModel viewModel, string bewaren, string deleten)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IndexDokterViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    //user instellen
                    LoggedInUser user = new LoggedInUser();
                    user.UserName = viewModel.Create.Email;
                    user.NormalizedUserName = viewModel.Create.Email.ToUpper();
                    user.Email = viewModel.Create.Email;
                    user.NormalizedEmail = user.Email.ToUpper();

                    //dokter instellen
                    user.Dokter = new Dokter() { Voornaam = viewModel.Create.Voornaam, Achternaam = viewModel.Create.Achternaam, LoggedUser = user, UserID = user.Id, PraktijkID = viewModel.Create.GeselecteerdePraktijk, SpecialisatieID = viewModel.Create.GeselecteerdeSpecialiteit };
                    user.LockoutEnabled = false;
                    user.EmailConfirmed = true;

                    var result = await _userManager.CreateAsync(user, viewModel.Create.Wachtwoord);
                    if (result.Succeeded)
                    {
                        //rol toevoegen
                        IdentityRole role = _context.Roles.FirstOrDefault(x => x.Name == "Dokter");
                        DbSet<IdentityUserRole<string>> roles = _context.UserRoles;
                        if (!roles.Any(us => us.UserId == user.Id && us.RoleId == role.Id))
                        {
                            roles.Add(new IdentityUserRole<string>() { UserId = user.Id, RoleId = role.Id });
                        }
                        int ok = await _context.SaveChangesAsync();
                        if (ok > 0)
                        {

                            return RedirectToAction("Index", "Admin");
                        }
                    }
                    else
                    {
                        //fouten toevoegen
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string
                                .Empty, error.Description);
                        }
                    }


                }
                // fouten toevoegen
                foreach (var error in ModelState.Values.SelectMany(x => x.Errors))
                {
                    ModelState.AddModelError(string
                        .Empty, error.ErrorMessage);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error");
            }

        }

        //gaat de dokter zijn attributen instellen aan de hand van selectie
        public Dokter DokterInstellen(IndexDokterViewModel viewModel, Dokter dokter)
        {
            if (viewModel.Update.Voornaam != null)
            {
                dokter.Voornaam = viewModel.Update.Voornaam;
            }
            if (viewModel.Update.Achternaam != null)
            {
                dokter.Achternaam = viewModel.Update.Achternaam;
            }
            if (viewModel.Update.GeselecteerdePraktijk > 0 && viewModel.Update.GeselecteerdePraktijk != null)
            {
                dokter.PraktijkID = (int)viewModel.Update.GeselecteerdePraktijk;
            }
            if (viewModel.Update.GeselecteerdeSpecialiteit > 0 && viewModel.Update.GeselecteerdeSpecialiteit != null)
            {
                dokter.SpecialisatieID = (int)viewModel.Update.GeselecteerdeSpecialiteit;
            }
            return dokter;
        }

        public async Task<ActionResult> Edit(IndexDokterViewModel model)
        {
            //dokter zoeken
            Dokter dokter = await _context.Dokters.Where(x => x.DokterID == model.Update.GeselecteerdeDokter).FirstOrDefaultAsync();
            if (ModelState.IsValid)
            {
                //user zoeken aan hand van UserID in dokter
                LoggedInUser user = await _userManager.FindByIdAsync(dokter.UserID);
               
                if (user != null)
                {
                    //kijken of email niet leeg is en zo de user herinstellen
                    if (model.Update.Email != null)
                    {
                        user.UserName = model.Update.Email;
                        user.NormalizedUserName = model.Update.Email.ToUpper();
                        user.Email = model.Update.Email;
                        user.NormalizedEmail = user.Email.ToUpper();
                    }
                    //hetzelfde geld voor wachtwoord
                    if (model.Update.Wachtwoord != null)
                    {
                        var nieuwWachtwoord = _userManager.PasswordHasher.HashPassword(user, model.Update.Wachtwoord);
                        user.PasswordHash = nieuwWachtwoord;
                    }
                    //user.dokter instellen aan de hand van een methode
                    user.Dokter = DokterInstellen(model, dokter);

                    //user updaten
                    await _userManager.UpdateAsync(user);
                    return RedirectToAction("Index", "Admin");
                }

            }
            //dan is er toch een fout gebeurt
            return RedirectToAction("Index", "Error");




        }


        // POST: DokterController/Delete/5
        public async Task<IActionResult> Delete(IndexDokterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                {
                    //dokter en user ophalen want user.dokter is leeg!
                    Dokter dokter = await _context.Dokters.Where(x => x.DokterID == viewModel.Update.GeselecteerdeDokter).SingleOrDefaultAsync();
                    LoggedInUser user = await _userManager.FindByIdAsync(dokter.UserID);
                    if (user != null)
                    {
                        //dokter verwijderen want de usermanager doet dit hier niet
                        _context.Dokters.Remove(dokter);
                        await _context.SaveChangesAsync();
                        //user verwijderen
                        await _userManager.DeleteAsync(user);
                    }
                }
                return RedirectToAction("Index", "Dokter");
            }

            return RedirectToAction("Index", "Error");
        }
    }
}
