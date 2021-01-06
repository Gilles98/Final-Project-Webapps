using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.ViewModels
{
    public class KlachtBeheerViewModel
    {
        public string Omschrijving { get; set; }
        public IEnumerable<SelectListItem> Klachten { get; set; }

        public int GeselecteerdeKlacht { get; set; }
    }


    public class KlachtBeheerValidator : AbstractValidator<KlachtBeheerViewModel>
    {
        public KlachtBeheerValidator()
        {
            RuleFor(x => x.Omschrijving).NotNull().WithMessage("Omschrijving mag niet leeg zijn");
            RuleFor(x => x.GeselecteerdeKlacht).NotEmpty().WithMessage("Er moet een klacht zijn geselecteerd!");
        }
    }
}