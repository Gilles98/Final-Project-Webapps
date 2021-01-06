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
using Microsoft.EntityFrameworkCore;

namespace Dokterspunt.Controllers
{
    [Authorize(Roles = "Patiënt")]
    public class AfspraakController : Controller
    {

        private readonly DokterspuntContext _context;
        private readonly UserManager<LoggedInUser> _userManager;
        public AfspraakController(DokterspuntContext context, UserManager<LoggedInUser> userManager)
        {
            this._userManager = userManager;
            _context = context;
        }
        // GET: AfspraakController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AfspraakController/Details/5

        [HttpPost]
        public async Task<string> CheckDate(DateTime date, int id, int afspraakId = 0)
        {
            string melding = "";

            //kijken of het tijdstip is gepasseerd dat geselecteerd is
            if (date < DateTime.Now)
            {
                return "Niet mogenlijk om een afspraak te plannen op een moment dat al geweest is";
            }

            //kijken of er een dokter id is geselecteerd
            if (id > 0)
            {
                var dokter = await _context.Dokters.Where(x => x.DokterID == id).SingleOrDefaultAsync();
                var tijdstippenDokters = await _context.Afspraken.Where(x => x.DokterID == id && afspraakId != x.AfspraakID).Select(x => x.AfspraakMoment).ToListAsync();
                if (tijdstippenDokters.Count > 0)
                {
                    foreach (DateTime tijdstip in tijdstippenDokters)
                    {
                        //is het tijdstip groter dan of gelijk aan een tijdstip in de database
                        if (date >= tijdstip)
                        {
                            //zit er een kwartier tussen
                            if (tijdstip.AddMinutes(15) >= date)
                            {
                           
                                melding = "er staat een consultatie gepland om: " + tijdstip.ToShortTimeString() + "\ngelieve een moment te kiezen 15 minuten vroeger of later";
                            }
                        }
                        //is het tijdstip kleiner dan of gelijk aan een tijdstip in de database
                        else
                        {
                            if (date <= tijdstip)
                            {
                                //zit er een kwartier tussen
                                if (date.AddMinutes(15) >= tijdstip)
                                {
                                    melding = "er staat een consultatie gepland om: " + tijdstip.Hour + " : " + tijdstip.Minute + "\ngelieve een moment te kiezen 15 minuten vroeger of later";
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                melding = "Selecteer eerst een arts om de beschikbaarheid te controleren!";
            }
            return melding;
        }
        public async Task<ActionResult> Create()
        {
            CreateAfspraakViewModel create = new CreateAfspraakViewModel()
            {
                Afspraaktypes = new SelectList(await _context.AfspraakTypes.ToListAsync(), "AfspraakTypeID", "SoortAfspraak"),
                Dokters = new SelectList(await _context.Dokters.Include(x => x.Praktijk).OrderBy(x => x.PraktijkID).ToListAsync(), "DokterID", "VolledigeGegevens"),
                Afspraak = new Models.Afspraak() { AfspraakMoment = DateTime.Now.Date },
                Klacht = new Models.Klacht(),
                Klacht2 = new Models.Klacht()
            };
            return View(create);
        }

        //gaat edit en delete posts regelen plus tokens
        public async Task<ActionResult> ProcessForm(UpdateAfspraakViewModel viewModel, string bewaren, string deleten)
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
        // POST: AfspraakController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateAfspraakViewModel create)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //haalt makkelijk de huidige user op
                    LoggedInUser user = await _userManager.GetUserAsync(HttpContext.User);
                    user.Patient = await _context.Patiënten.Where(x => x.UserID == user.Id).SingleOrDefaultAsync();

                    //check of de database al deze klacht bevat om duplicaat te vermijden
                    var klachten = await _context.Klachten.Where(x => x.Omschrijving == create.Klacht.Omschrijving).ToListAsync();
                    if (klachten.Count == 0)
                    {
                        Klacht klacht = new Klacht() { Omschrijving = create.Klacht.Omschrijving };
                        await _context.Klachten.AddAsync(klacht);
                        ///al toevoegen omdat ik anders een exception krijg
                        await _context.SaveChangesAsync();
                        await _context.KlachtenPatiënten.AddAsync(new KlachtPatiënt() { KlachtID = klacht.KlachtID, PatiëntID = user.Patient.PatiëntID });
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        var checkKlachtenPatiënt = await _context.KlachtenPatiënten.Where(x => x.KlachtID == klachten[0].KlachtID && x.PatiëntID == user.Patient.PatiëntID).ToListAsync();
                       //voorkomt dat in de tabel KlachtenPatient er geen dubbele records komen dus dat als een patient meerdere keren dezelfde klacht
                       //ingeeft bij verschillende afspraken deze relatie nooit een duplicaat kan worden en dus onnodig ruimte gaat nemen
                        if (checkKlachtenPatiënt.Count <= 0)
                        {
                            await _context.KlachtenPatiënten.AddAsync(new KlachtPatiënt() { KlachtID = klachten[0].KlachtID, PatiëntID = user.Patient.PatiëntID });
                            await _context.SaveChangesAsync();
                        }
                        
                    }
                    //check of de optionele 2de klacht is ingevuld en of deze al in de database staat
                    if (create.Klacht2.Omschrijving != null)
                    {
                        klachten = _context.Klachten.Where(x => x.Omschrijving == create.Klacht2.Omschrijving).ToList();
                        if (klachten.Count == 0)
                        {
                            Klacht klacht = new Klacht() { Omschrijving = create.Klacht2.Omschrijving };
                            await _context.Klachten.AddAsync(klacht);
                            ///al toevoegen omdat ik anders een exception krijg
                            await _context.SaveChangesAsync();
                            await _context.KlachtenPatiënten.AddAsync(new KlachtPatiënt() { KlachtID = klacht.KlachtID, PatiëntID = user.Patient.PatiëntID });
                            await _context.SaveChangesAsync();

                        }
                        else
                        {
                            var checkKlachtenPatiënt = await _context.KlachtenPatiënten.Where(x => x.KlachtID == klachten[0].KlachtID && x.PatiëntID == user.Patient.PatiëntID).ToListAsync();
                            if (checkKlachtenPatiënt.Count <= 0)
                            {
                                //voorkomt dat in de tabel KlachtenPatient er geen dubbele records komen dus dat als een patient meerdere keren dezelfde klacht
                                //ingeeft bij verschillende afspraken deze relatie nooit een duplicaat kan worden en dus voorkomen wordt dat deze onnodig ruimte gaat innemen
                                await _context.KlachtenPatiënten.AddAsync(new KlachtPatiënt() { KlachtID = klachten[0].KlachtID, PatiëntID = user.Patient.PatiëntID });
                                await _context.SaveChangesAsync();
                            }
                        }
                       
                    }
                    //wanneer alles correct is verlopen
                    Afspraak afspraak = new Afspraak() { PatiëntID = user.Patient.PatiëntID, AfspraakMoment = create.Afspraak.AfspraakMoment, AfspraakTypeID = create.GeselecteerdeAfspraakType, DokterID = create.GeselecteerdeDokter };
                    await _context.Afspraken.AddAsync(afspraak);
                    var ok = await _context.SaveChangesAsync();
                    if (ok > 0)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
                return RedirectToAction(nameof(Create));

            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: AfspraakController/Edit/5
        public async Task<ActionResult> Beheer(int id)
        {
            if (id <= 0)
            {   
                return RedirectToAction("Index", "Error");
            }
            //hier ga ik de user ophalen in de httpcontext omdat de ID dat ik meegeef van een afspraak is en ik alsnog de user moet hebben
            //dit verkort de code ipv dat ik ook de afpsraak eerst nog moet zoeken
            LoggedInUser user = await _userManager.GetUserAsync(HttpContext.User);
            Patiënt patiënt = await _context.Patiënten.Where(x => x.UserID == user.Id).SingleOrDefaultAsync();
            //de database slaagt op volgorde op dus kan ik via index klachten eruit halen
            UpdateAfspraakViewModel vm = new UpdateAfspraakViewModel()
            {
                Afspraak = await _context.Afspraken.Where(x =>x.AfspraakID == id).SingleOrDefaultAsync(),
                Dokters = new SelectList(await _context.Dokters.Include(x => x.Praktijk).OrderBy(x => x.PraktijkID).ToListAsync(), "DokterID", "VolledigeGegevens"),
                GeselecteerdeDokter = await _context.Afspraken.Where(x =>x.AfspraakID == id).Select(x => x.DokterID).SingleOrDefaultAsync(),
                Afspraaktypes = new SelectList(await _context.AfspraakTypes.ToListAsync(), "AfspraakTypeID", "SoortAfspraak"),
                GeselecteerdeAfspraakType = await _context.Afspraken.Where(x => x.AfspraakID == id).Select(x => x.AfspraakTypeID).SingleOrDefaultAsync()
            };
            
            return View(vm);
        }

        // POST: AfspraakController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UpdateAfspraakViewModel vm)
        {
            try
            {
                //geen modelstate is valid omdat dit bij beheer allemaal optionele velden zijn waarvan standaard al een value is ingevuld
                vm.Afspraak.DokterID = vm.GeselecteerdeDokter;
                vm.Afspraak.AfspraakTypeID = vm.GeselecteerdeAfspraakType;
                _context.Afspraken.Update(vm.Afspraak);
                int ok = await _context.SaveChangesAsync();
                if (ok > 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Index", "Error");
               
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }


        // POST: AfspraakController/Delete/5
        public async Task<ActionResult> Delete(UpdateAfspraakViewModel vm)
        {
            try
            {
                ///ook hier geen modelstate is geldig vanwege dezelfde redene bij de edit methode
                _context.Remove(vm.Afspraak);
                int ok = await _context.SaveChangesAsync();
                if (ok > 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Index", "Error");
            }
            catch
            {
                return View();
            }
        }
    }
}
