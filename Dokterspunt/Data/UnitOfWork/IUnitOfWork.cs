using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dokterspunt.Repository;
namespace Dokterspunt.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
            public IAfspraakRepository AfspraakRepository { get; }
            public Task Save();   
    }
}
