using Dokterspunt.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.Models
{
    public class Patiënt
    {
        public int PatiëntID { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }

        [NotMapped]
        [Display(Name = "Naam van de patiënt")]
        public string VolledigePatiënt => $"{Voornaam} {Achternaam}";
        public string Gemeente { get; set; }
        public string Straat { get; set; }
        public string HuisNr { get; set; }
        public int Postcode { get; set; }

        [ForeignKey("LoggedUser")]
        public string UserID { get; set; }

        //navigatie
        public LoggedInUser LoggedUser { get; set; }
        public ICollection<MedischDossier> MedischDossier { get; set; }
        public List<Afspraak>Afspraken { get; set; }
        public List<KlachtPatiënt>KlachtenPatiënten { get; set; }

        ///tostring aanpassen

    }
}
