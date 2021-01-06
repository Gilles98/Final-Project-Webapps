using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.Models
{
    public class KlachtPatiënt
    {
        public int KlachtPatiëntID { get; set; }
        public int PatiëntID { get; set; }

        public int KlachtID{ get; set; }
        //navigatie
        public Patiënt Patiënt { get; set; }
        public Klacht Klacht { get; set; }

    }
}
