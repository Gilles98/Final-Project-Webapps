using Dokterspunt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.ViewModels
{
    public class IndexViewModel
    {
        public Patiënt Patiënt { get; set; }

        public List<Afspraak> Afspraken { get; set; }
        public Dokter Dokter { get; set; }
    }
}
