using Dokterspunt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.Repository
{
    public interface IAfspraakRepository
    {
        IQueryable<Afspraak> GetAll();
        Task<Afspraak> GetById(int id);
        void Create(Afspraak entity);
        void Update(Afspraak entity);
        void Delete(Afspraak entity);


    }
}
