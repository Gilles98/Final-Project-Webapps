using Dokterspunt.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.ViewModels
{
    public class UpdateAfspraakViewModel
    {
     
            public Afspraak Afspraak { get; set; }
            public IEnumerable<SelectListItem> Dokters { get; set; }
            public IEnumerable<SelectListItem> Afspraaktypes { get; set; }

            public int GeselecteerdeAfspraakType { get; set; }

            public int GeselecteerdeDokter { get; set; }
        
    }
}
