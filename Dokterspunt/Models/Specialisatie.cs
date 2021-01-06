using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.Models
{
    public class Specialisatie
    {
        public int SpecialisatieID { get; set; }
        public string Omschrijving { get; set; }

        //navigatie

        public ICollection<Dokter>Dokters { get; set; }

        public override string ToString()
        {
            return Omschrijving.ToString();
        }
    }
}
