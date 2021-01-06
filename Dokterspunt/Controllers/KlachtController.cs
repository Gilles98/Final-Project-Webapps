using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dokterspunt.Areas.Identity.Data;
using Dokterspunt.Data;
using Dokterspunt.Models;
using Dokterspunt.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dokterspunt.Controllers
{
    [Authorize(Roles = "Patiënt")]
    public class KlachtController : Controller
    {
        // GET: KlachtController
        private readonly DokterspuntContext _context;
        private readonly UserManager<LoggedInUser> _userManager;
        public KlachtController(DokterspuntContext context, UserManager<LoggedInUser> userManager)
        {
            this._userManager = userManager;
            _context = context;
        }
        public static Patiënt Patiënt = null; 
        public async Task<ActionResult> Index()
        {
            LoggedInUser user = await _userManager.GetUserAsync(HttpContext.User);
            Patiënt = _context.Patiënten.Where(x => x.UserID == user.Id).SingleOrDefault();
            var klachtenPatiënten = _context.KlachtenPatiënten.Where(x => x.PatiëntID == Patiënt.PatiëntID);
            List<Klacht> Klachten = new List<Klacht>();
            foreach (var item in klachtenPatiënten)
            {
                var klacht = _context.Klachten.Where(x => x.KlachtID == item.KlachtID).SingleOrDefault();
                Klachten.Add(klacht);
  
            }
            KlachtBeheerViewModel viewModel = new KlachtBeheerViewModel()
            {
                Klachten = new SelectList(Klachten, "KlachtID", "Omschrijving"),

            };
            return View(viewModel);
        }
        /// ajax
        [HttpPost]
        public string VulVeld(KlachtBeheerViewModel viewModel, int id = 0)
        {
            var obj = viewModel;
            string omschrijving = "";
            if (id > 0)
            {
                omschrijving =  _context.Klachten.Where(x => x.KlachtID == id).Select(x => x.Omschrijving).SingleOrDefault();
            }
            return omschrijving;
        }
        ///create wordt geregeld bij het maken van een afspraak

        // POST: KlachtController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(KlachtBeheerViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid && !string.IsNullOrWhiteSpace(viewModel.Omschrijving))
                {
                     Klacht klacht = await _context.Klachten.FindAsync(viewModel.GeselecteerdeKlacht);
                     klacht.Omschrijving = viewModel.Omschrijving;
                    _context.Klachten.Update(klacht);
                    int ok = await _context.SaveChangesAsync();
                    if (ok <= 0)
                    {
                        return RedirectToAction("Index", "Error");
                    }
                    
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }
        public async Task<IActionResult> ProcessForm(KlachtBeheerViewModel viewModel, string bewaren, string deleten)
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

        public  bool ControleKlachten(Klacht klacht)
        {
            var klachten = _context.KlachtenPatiënten.Where(x => x.KlachtID == klacht.KlachtID).ToList();
            if (klachten.Count > 1)
            {
                return false;
            }
            return true;
        }
        // POST: KlachtController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(KlachtBeheerViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Klacht klacht = await _context.Klachten.FindAsync(viewModel.GeselecteerdeKlacht);
                    if (ControleKlachten(klacht))
                    {
                        _context.Klachten.Remove(klacht);
                    }
                    else
                    {
                        var tussenTabelFks = _context.KlachtenPatiënten.Where(x => x.KlachtID == klacht.KlachtID && x.PatiëntID == Patiënt.PatiëntID);
                        _context.KlachtenPatiënten.RemoveRange(tussenTabelFks);
                    }
                    
                    int ok = await _context.SaveChangesAsync();
                    if (ok <= 0)
                    {
                        return RedirectToAction("Index", "Error");
                    }

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
