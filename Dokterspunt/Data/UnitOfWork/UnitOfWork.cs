using Dokterspunt.Models;
using Dokterspunt.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DokterspuntContext _context;
        public IAfspraakRepository _afspraakRepository;


        public UnitOfWork(DokterspuntContext context)
        {
            _context = context;
        }

        public IAfspraakRepository AfspraakRepository
        {
            get
            {
                if (_afspraakRepository == null)
                {
                    _afspraakRepository = new AfspraakRepository(_context);
                }
                return _afspraakRepository;
            }
        }


        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

    }
}
