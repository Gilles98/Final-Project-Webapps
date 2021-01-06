using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Dokterspunt.Models;
using Dokterspunt.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Dokterspunt.Data;
using Microsoft.EntityFrameworkCore;
using Dokterspunt.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Dokterspunt.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly DokterspuntContext _context;
        private readonly UserManager<LoggedInUser> _userManager;
        public HomeController(DokterspuntContext context, UserManager<LoggedInUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //datums afspraken checken en filteren op kwartier
        public async Task CheckAfspraken()
        {
            var lijstAfsprakenVerwijderen = await _context.Afspraken.Where(x => x.AfspraakMoment.AddMinutes(15) < DateTime.Now).ToListAsync();
                if (lijstAfsprakenVerwijderen.Count > 0)
                {
                    _context.Afspraken.RemoveRange(lijstAfsprakenVerwijderen);
                    await _context.SaveChangesAsync();
                }   
        }
        public async Task<ActionResult> Index(IndexViewModel viewModel)
        {
            if (User.Identity.Name != null)
            {
                ///doet controle of dat een afspraak is gepasseerd zodra de gebruiker op zijn afspraken pagina zit.
                ///geld meteen voor alle afspraken van iedere gebruiker
                await CheckAfspraken();
                string userID = _userManager.GetUserId(User);
                if (User.IsInRole("Admin") || User.IsInRole("Dokter"))
                {
                    viewModel.Dokter = await _context.Dokters.Where(x => x.UserID == userID).FirstOrDefaultAsync();
                    viewModel.Afspraken = await _context.Afspraken.Include(x => x.AfspraakType).Include(x => x.Patiënt).Where(x => x.DokterID == viewModel.Dokter.DokterID).ToListAsync();
                }
                if (User.IsInRole("Patiënt"))
                {
                    viewModel.Patiënt = await _context.Patiënten.Where(x => x.UserID == userID).FirstOrDefaultAsync();
                    viewModel.Afspraken = await _context.Afspraken.Include(x => x.AfspraakType).Include(x => x.Dokter).ThenInclude(x => x.Praktijk).Where(x => x.PatiëntID == viewModel.Patiënt.PatiëntID).ToListAsync();
                }
                return View(viewModel);
            }
            return View();
        }
      
        public ActionResult Privacy()
        {

            return View();
        }
        public ActionResult Corona_Maatregelen()
        {
            return View();
        }


        public async Task<ActionResult> Teams()
        {
            ListTeamsViewModel viewModel = new ListTeamsViewModel();
            viewModel.Praktijken = await _context.Praktijken.Include(x => x.Dokters).ThenInclude(y => y.Specialisatie).ToListAsync();


            return View(viewModel);
        }

        public ActionResult Locatie()
        {
            DropdownLocationViewModel viewModel = new DropdownLocationViewModel()
            {
                Praktijk = new Praktijk(),
                Praktijken = new SelectList(_context.Praktijken, "PraktijkID", "VolledigAdress"),
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<ActionResult> Locatie(DropdownLocationViewModel viewModel)
        {
            ///opnieuw instellen van de praktijken aangezien we in dezelfde view blijven
            viewModel.Praktijken = new SelectList(_context.Praktijken, "PraktijkID", "VolledigAdress");
            viewModel.Praktijk = await _context.Praktijken.Where(x => x.PraktijkID == viewModel.GeselecteerdePraktijk).SingleOrDefaultAsync();
            return View(viewModel);
        }
    }
}
