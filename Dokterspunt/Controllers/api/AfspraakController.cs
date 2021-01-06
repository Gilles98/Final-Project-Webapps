using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dokterspunt.Data;
using Dokterspunt.Data.UnitOfWork;
using Dokterspunt.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dokterspunt.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AfspraakController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public AfspraakController(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
            
        }
        // GET: api/<AfspraakController>

        //alle afspraken
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Afspraak>>> GetAll()
        {
            return await _uow.AfspraakRepository.GetAll().ToListAsync();
        }

        // GET api/<AfspraakController>/5
        [HttpGet("{id}")]

        //een afspraak op id
        public async Task<ActionResult<Afspraak>> GetOne(int id)
        {
            var afspraak = await _uow.AfspraakRepository.GetById(id);
            if (afspraak == null)
            {
                return NotFound();
            }
            return afspraak;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("Overzicht")]
        public async Task<ActionResult<IEnumerable<Afspraak>>> GetAfsprakenlijst()
        {
            //vanwege de property VolledigeGegevens in het model Dokter moet ik de praktijk includen om een null fout te voorkomen
            return await _uow.AfspraakRepository.GetAll().Include(b => b.Dokter).ThenInclude(x => x.Praktijk).Include(x => x.Patiënt).ToListAsync();
        }

        //extra keuze
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("Een Of Meer")]
        public async Task<ActionResult<IEnumerable<Afspraak>>>OneOrMore(bool trueIsAlleAfsprakenFalseIsEenMetId, int id = 0)
        {
            if (trueIsAlleAfsprakenFalseIsEenMetId)
            {
                return await GetAll(); 
            }

            else if (!trueIsAlleAfsprakenFalseIsEenMetId && id > 0)
            {
                List<Afspraak> soloAfspraken = new List<Afspraak>();
                soloAfspraken.Add(await _uow.AfspraakRepository.GetById(id));
                return soloAfspraken;
            }
            return BadRequest();
        }

        //gaat een random afspraak returnen
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("Random")]
        public async Task<ActionResult<Afspraak>> GetRandom()
        {
            var afspraken = await _uow.AfspraakRepository.GetAll().ToListAsync();
            return afspraken[new Random().Next(afspraken.Count())];
        }

        // POST api/<AfspraakController>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Afspraak>> PostAfspraak(Afspraak afspraak)
        {
            _uow.AfspraakRepository.Create(afspraak);
            await _uow.Save();
            return CreatedAtAction("GetOne", new { id = afspraak.AfspraakID }, afspraak);
        }

        // PUT api/<AfspraakController>/5
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutAfspraak(int id, Afspraak afspraak)
        {
            if (id != afspraak.AfspraakID)
            {
                return BadRequest();
            }
            _uow.AfspraakRepository.Update(afspraak);
            await _uow.Save();
            return NoContent();

        }

        // DELETE api/<AfspraakController>/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Afspraak>> DeleteAfspraak(int id)
        {
            Afspraak afspraak = await _uow.AfspraakRepository.GetById(id);
            if (afspraak == null)
            {
                return NotFound();
            }

            _uow.AfspraakRepository.Delete(afspraak);
            await _uow.Save();

            return NoContent();
        }
    }
}
