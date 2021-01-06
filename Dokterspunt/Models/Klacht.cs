using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.Models
{
    public class Klacht
    {
        public int KlachtID { get; set; }
        public string Omschrijving { get; set; }
        //navigatie
        public List<KlachtPatiënt> KlachtenPatiënten { get; set; }

    }


}
