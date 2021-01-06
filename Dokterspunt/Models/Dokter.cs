using Dokterspunt.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.Models
{
    public class Dokter
    {
        public int DokterID { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public int SpecialisatieID { get; set; }
        public int PraktijkID { get; set; }

        [ForeignKey("LoggedUser")]
        public string UserID { get; set; }

        [NotMapped]
        [Display(Name = "Gegevens van de dokter")]
        public string VolledigeGegevens => $"{Voornaam} {Achternaam} - Praktijk: {Praktijk.Gemeente} {Praktijk.Straat} {Praktijk.HuisNr}";

        //navigatie
        public LoggedInUser LoggedUser { get; set; }
        public List<Afspraak>Afspraken { get; set; }
        public Specialisatie Specialisatie { get; set; }
        public Praktijk Praktijk { get; set; }
    }
}
