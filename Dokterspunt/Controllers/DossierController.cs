using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dokterspunt.Data;
using Dokterspunt.Models;
using Dokterspunt.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dokterspunt.Controllers
{
    [Authorize(Roles = "Dokter")]
    public class DossierController : Controller
    {
        // GET: DossierController
        private readonly DokterspuntContext _context;
        public DossierController(DokterspuntContext context)
        {
            _context = context;

        }
        public async Task <ActionResult> Index(int id)
        {
            MedischDossierViewModel viewModel = new MedischDossierViewModel();
            viewModel.Patiënt = await _context.Patiënten.FindAsync(id);

            //voor de processform task goed te laten lopen
            viewModel.Dossier = _context.MedischeDossiers.Where(x => x.PatiëntID == viewModel.Patiënt.PatiëntID).SingleOrDefault();
            if (viewModel.Dossier == null)
            {
                ///create
                viewModel.Dossier = new MedischDossier() { PatiëntID = viewModel.Patiënt.PatiëntID };
            }
            else
            {
                viewModel.Verleden = viewModel.Dossier.MedischVerleden;
            }
            return View(viewModel);
        }

        // GET: DossierController/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProcessForm(MedischDossierViewModel viewModel)
        {
           
            if (viewModel.Dossier.ID > 0)
            {
                var edit = await Edit(viewModel);
                if (edit != null)
                {
                    return edit;
                }
            }
            else
            {
                var create = await Create(viewModel);
                if (create != null)
                {
                    return create;
                }
            }
            return RedirectToAction("Index", "Error");
        }

        // POST: DossierController/Create
        public async Task<ActionResult> Create(MedischDossierViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //verleden moest apart ingesteld worden eerst
                    viewModel.Dossier.MedischVerleden = viewModel.Verleden;
                    await _context.AddAsync(viewModel.Dossier);
                    int ok = await _context.SaveChangesAsync();
                    if (ok > 0)
                    {
                        ///succes
                        return RedirectToAction("Index", "Home");
                    }
                    return RedirectToAction("Index", "Error");
                }
                return RedirectToAction("Index", new { id = viewModel.Dossier.PatiëntID });
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }

        public async Task<ActionResult> Edit(MedischDossierViewModel viewModel)
        {
                try
                {
                    if (ModelState.IsValid)
                    {
                    //verleden moest apart ingesteld worden eerst
                    viewModel.Dossier.MedischVerleden = viewModel.Verleden;
                        _context.Update(viewModel.Dossier);
                        int ok = await _context.SaveChangesAsync();
                        if (ok > 0)
                        {
                        //succes
                        return RedirectToAction("Index", "Home");
                        }
                        return RedirectToAction("Index", "Error");
                    }
                    return RedirectToAction("Index", new { id = viewModel.Dossier.PatiëntID });
                }
                catch
                {
                    return RedirectToAction("Index", "Error");
                }

        }
    }
}
