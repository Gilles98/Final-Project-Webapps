using Dokterspunt.Data;
using Dokterspunt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.Repository
{
    public class AfspraakRepository : IAfspraakRepository
    {
        private readonly DokterspuntContext _context;

        public AfspraakRepository(DokterspuntContext context)
        {
            _context = context;
        }
        public IQueryable<Afspraak> GetAll()
         {
            return _context.Set<Afspraak>();
        }

        public async Task<Afspraak> GetById(int id)
        {
            return await _context.Set<Afspraak>().FindAsync(id);
        }

        public void Create(Afspraak entity)
        {
            _context.Set<Afspraak>().Add(entity);
        }

        public void Update(Afspraak entity)
        {
            _context.Set<Afspraak>().Update(entity);
        }

        public void Delete(Afspraak entity)
        {
            _context.Set<Afspraak>().Remove(entity);
        }
    }
}
