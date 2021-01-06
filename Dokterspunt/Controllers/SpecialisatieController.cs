using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dokterspunt.Data;
using Dokterspunt.Models;
using Dokterspunt.ViewModels;
using Dokterspunt.ViewModels.ModelsForViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Dokterspunt.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SpecialisatieController : Controller
    {
        private readonly DokterspuntContext _context;
        public SpecialisatieController(DokterspuntContext context)
        {
            _context = context;

        }
        public ActionResult Index(IndexSpecialisatieViewModel viewModel)
        {
            viewModel = new IndexSpecialisatieViewModel()
            {
                Create = new CreateSpecialisatieModel(),
                Update = new UpdateSpecialisatieModel()
                {
                    Specialisaties = new SelectList(_context.Specialisaties, "SpecialisatieID", "Omschrijving"),
                    GeselecteerdeSpecialisatie = 0,
                }
            };
            return View(viewModel);
        }

        //gaat de post en token regelen voor bewaren en of deleten
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessForm(IndexSpecialisatieViewModel viewModel, string bewaren, string deleten)
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
            return  RedirectToAction("Index", "Error");
        }


        // POST: Specialisatie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IndexSpecialisatieViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _context.Specialisaties.AddAsync(new Specialisatie()
                    {
                        Omschrijving = viewModel.Create.Beschrijving
                    }
                    );
                    await _context.SaveChangesAsync();
              
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }




   
        public async Task<ActionResult> Edit(IndexSpecialisatieViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Specialisatie specialisatie = _context.Specialisaties.Where(x => x.SpecialisatieID == viewModel.Update.GeselecteerdeSpecialisatie).SingleOrDefault();
                    specialisatie.Omschrijving = viewModel.Update.Beschrijving;
                   _context.Specialisaties.Update(specialisatie);
                    await _context.SaveChangesAsync(); 
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Delete(IndexSpecialisatieViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var alleSpecialisaties = _context.Specialisaties.Count();
                    //ervoor zorgen dat de laatste specialisatie niet kan worden verwijderd
                    if (alleSpecialisaties <= 1)
                    {
                      return RedirectToAction(nameof(Index));
                    }
                    ///dokters herindelen
                    var dokters = await _context.Dokters.Where(x => x.SpecialisatieID == viewModel.Update.GeselecteerdeSpecialisatie).ToListAsync();
                    if (dokters != null)
                    {
                        var specialisaties = await _context.Specialisaties.Where(x => x.SpecialisatieID != viewModel.Update.GeselecteerdeSpecialisatie).ToListAsync();

                       
      

                        Random random = new Random();
                        for (int i = 0; i <= dokters.Count-1; i++)
                        {
                            int resultaatRandom = random.Next(specialisaties.Count);
                            dokters[i].SpecialisatieID = specialisaties[resultaatRandom].SpecialisatieID;
                        }
                        _context.Dokters.UpdateRange(dokters);
                        int ok = await _context.SaveChangesAsync();
                        if (ok > 0)
                        {
                            Specialisatie verwijderen = _context.Specialisaties.Where(x => x.SpecialisatieID == viewModel.Update.GeselecteerdeSpecialisatie).FirstOrDefault();
                            _context.Specialisaties.Remove(verwijderen);
                            await _context.SaveChangesAsync();

                        }
                        else
                        {
                            return new RedirectToActionResult("Index", "Error", null);
                        }
                        
                    }
                    else
                    {
                        Specialisatie verwijderen = _context.Specialisaties.Where(x => x.SpecialisatieID == viewModel.Update.GeselecteerdeSpecialisatie).FirstOrDefault();
                        _context.Specialisaties.Remove(verwijderen);
                        await _context.SaveChangesAsync();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return new RedirectToActionResult("Index", "Error", null);
            }
        }
    }
}
