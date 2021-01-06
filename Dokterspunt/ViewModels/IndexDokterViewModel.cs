using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.ViewModels
{
    public class IndexDokterViewModel
    {
        public CreateDokterModel Create { get; set; }
        public UpdateDokterModel Update { get; set; }
    }
}
