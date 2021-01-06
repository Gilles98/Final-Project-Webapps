using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dokterspunt.Areas.Identity.Data;
using Dokterspunt.Data;
using Dokterspunt.Models;
using Dokterspunt.ViewModels.ModelsForViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Dokterspunt.Controllers
{
    [Authorize(Roles ="Admin")]
    public class PraktijkController : Controller
    {
        // GET: PraktijkController

        private readonly DokterspuntContext _context;
        private readonly UserManager<LoggedInUser> _userManager;
        public PraktijkController(DokterspuntContext context, UserManager<LoggedInUser>userManager)
        {
            _userManager = userManager;
            _context = context;

        }
        public async Task<ActionResult> Index(IndexPraktijkViewModel viewModel)
        {
            LoggedInUser gebruiker = await _userManager.FindByNameAsync(User.Identity.Name);
            gebruiker.Dokter = await _context.Dokters.Where(x => x.UserID == gebruiker.Id).Include(x => x.Praktijk).SingleOrDefaultAsync();
            viewModel = new IndexPraktijkViewModel()
            {
                Create = new CreatePraktijkModel(),
                Update = new UpdatePraktijkModel()
                {
                    //praktijk van de admin eruit filteren
                    Praktijken = new SelectList(await _context.Praktijken.Where(x => x.PraktijkID != gebruiker.Dokter.PraktijkID).ToListAsync(), "PraktijkID", "VolledigAdress")
                },
            };



            return View(viewModel);
        }

        [HttpPost]
        public async Task<JsonResult> ZoekPraktijk(int id)
        {
            Praktijk result = await _context.Praktijken.Where(x => x.PraktijkID == id).SingleOrDefaultAsync();
            return Json(result);
        }
        
        //gaat de create, edit en delete posts regelen plus het token 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProcessForm(IndexPraktijkViewModel viewModel, string bewaren, string deleten, string opslagen)
        {
         
            if (!string.IsNullOrEmpty(opslagen))
            {
                var create = Create(viewModel);
                if (create != null)
                {
                    return await create;
                }
            }
            if (!string.IsNullOrEmpty(bewaren))
            {
                var edit = Edit(viewModel);
                if (edit != null)
                {
                    return await edit;
                }
            }
            if (!string.IsNullOrEmpty(deleten))
            {
                var delete = Delete(viewModel);
                if (delete != null)
                {
                    return await delete;
                }
            }
            return RedirectToAction("Index", "Error");
        }



        public async Task<ActionResult> Create(IndexPraktijkViewModel viewModel)
        {
            try
            {

                    if (ModelState.IsValid)
                    {

                    //deze moeten strings zijn want ik moet het punt met een komma vervangen omdat c# deze anders niet herkent.
                    //properties van het model staan op dit moment ook op string ipv float,double of decimal
                    await _context.Praktijken.AddAsync(new Praktijk()
                        {
                            Breedtegraad = viewModel.Create.Breedtegraad,
                            Lengtegraad = viewModel.Create.Lengtegraad,
                            Gemeente = viewModel.Create.Gemeente,
                            Straat = viewModel.Create.Straat,
                            HuisNr = viewModel.Create.HuisNr,
                            Postcode = viewModel.Create.Postcode
                        });
                        await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index", viewModel);
            }

            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        public Praktijk PraktijkInstellen(IndexPraktijkViewModel viewModel, Praktijk praktijk)
        {

            

            //deze moeten strings zijn want ik moet het punt met een komma vervangen omdat c# deze anders niet herkent.
            //properties van het model staan op dit moment ook op string ipv float,double of decimal
            if (viewModel.Update.Lengtegraad > 0)
            {
                praktijk.Lengtegraad = viewModel.Update.Lengtegraad;
            }
            if (viewModel.Update.Breedtegraad > 0)
            {
                praktijk.Breedtegraad = viewModel.Update.Breedtegraad;
            }

            if (viewModel.Update.Gemeente != null)
            {
                praktijk.Gemeente = viewModel.Update.Gemeente;
            }
           
            if (viewModel.Update.Straat != null)
            {
                praktijk.Straat = viewModel.Update.Straat;
            }

            if (viewModel.Update.HuisNr != null)
            {
                praktijk.HuisNr = viewModel.Update.HuisNr;
            }

            if (viewModel.Update.Postcode != null)
            {
                //gaf fout zonder de parse
                praktijk.Postcode = (int)viewModel.Update.Postcode;
            }
           
            return praktijk;
        }

        // GET: PraktijkController/Edit/5


        public async Task<ActionResult> Edit(IndexPraktijkViewModel viewModel)
        {
            try
            {
                Praktijk praktijk = _context.Praktijken.Where(x => x.PraktijkID == viewModel.Update.GeselecteerdePraktijk).SingleOrDefault();
                if (ModelState.IsValid)
                {
                    praktijk = PraktijkInstellen(viewModel, praktijk);
                    _context.Praktijken.Update(praktijk);
                    int ok = await _context.SaveChangesAsync();
                    if (ok > 0)
                    {
                        return RedirectToAction("Index");
                    }
                    else
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

        // GET: PraktijkController/Delete/5
 
        public async Task<ActionResult> Delete(IndexPraktijkViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    {
                        Praktijk praktijk = _context.Praktijken.Where(x => x.PraktijkID == viewModel.Update.GeselecteerdePraktijk).SingleOrDefault();
                        if (praktijk != null)
                        {
                            _context.Praktijken.Remove(praktijk);
                            await _context.SaveChangesAsync();
                            return RedirectToAction("Index", "Praktijk");
                        }
                   
                    }

                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error");
            }
           
           
            
        }
    }
}
