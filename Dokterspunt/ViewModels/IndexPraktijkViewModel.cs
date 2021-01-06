using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.ViewModels.ModelsForViewModels
{
    public class IndexPraktijkViewModel
    {
        public CreatePraktijkModel Create { get; set; }
        public UpdatePraktijkModel Update { get; set; }

        public bool Completed { get; set; }
    }
}
