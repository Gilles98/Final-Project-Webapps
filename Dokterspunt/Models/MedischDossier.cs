using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.Models
{
    public class MedischDossier
    {
        public int ID{ get; set; }
        public string Diagnose { get; set; }

        public string MedischVerleden { get; set; }
        public string Medicatie { get; set; }
        public int PatiëntID { get; set; }
        //navigatie
        public Patiënt Patiënt { get; set; }
}
}
