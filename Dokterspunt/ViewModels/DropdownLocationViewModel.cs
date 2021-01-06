using Dokterspunt.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.ViewModels
{
    public class DropdownLocationViewModel
    {
        public Praktijk Praktijk { get; set; }
        public IEnumerable<SelectListItem> Praktijken { get; set; }

        public int GeselecteerdePraktijk{ get; set; }

    }
}
