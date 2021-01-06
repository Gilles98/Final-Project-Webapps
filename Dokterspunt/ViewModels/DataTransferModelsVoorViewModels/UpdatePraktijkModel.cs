using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.ViewModels.ModelsForViewModels
{
    public class UpdatePraktijkModel
    {

        public string Gemeente { get; set; }


        public string Straat { get; set; }


        public string HuisNr { get; set; }

        public int? Postcode { get; set; }

        public Decimal Lengtegraad { get; set; }
        public Decimal Breedtegraad { get; set; }

        public IEnumerable<SelectListItem> Praktijken { get; set; }

        public int GeselecteerdePraktijk { get; set; }
    }

    public class UpdatePraktijkValidator: AbstractValidator<UpdatePraktijkModel>
    {
        public UpdatePraktijkValidator()
        {
            RuleFor(x => x.GeselecteerdePraktijk).NotEmpty().WithMessage("Er moet een praktijk zijn geselecteerd!");
        }
    }
}
