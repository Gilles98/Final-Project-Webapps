using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.Models
{
    public class Afspraak
    {
        public int AfspraakID { get; set; }
        public int PatiëntID { get; set; }
        public int DokterID { get; set; }
        public int AfspraakTypeID { get; set; }

        //was eerst date en time op erd
        public DateTime AfspraakMoment { get; set; }
 

        //navigatie
        public AfspraakType AfspraakType { get; set; }

        public Patiënt Patiënt { get; set; }

        public Dokter Dokter { get; set; }
    }
}
