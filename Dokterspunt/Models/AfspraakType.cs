using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.Models
{
    public class AfspraakType
    {
        public int AfspraakTypeID { get; set; }
        public string SoortAfspraak { get; set; }

        //navigatie
        public ICollection<Afspraak>Afspraken { get; set; }
    }
}
