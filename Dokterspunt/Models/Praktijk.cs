using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.Models
{
    public class Praktijk
    {
        public int PraktijkID { get; set; }
        public string Gemeente { get; set; }
        public string Straat { get; set; }
        public string HuisNr { get; set; }
        public int Postcode { get; set; }
        public Decimal Breedtegraad { get; set; }
        public Decimal Lengtegraad { get; set; }

       [NotMapped]
        public string VolledigAdress => $"{Gemeente} - {Straat} {HuisNr}";

        //navigatie
        public ICollection<Dokter>Dokters { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
