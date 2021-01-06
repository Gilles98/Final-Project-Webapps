using Dokterspunt.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.ViewModels
{
    public class CreateAfspraakViewModel
    {
        public Afspraak Afspraak { get; set; }

        public Klacht Klacht { get; set; }

        public Klacht Klacht2 { get; set; }
        public IEnumerable<SelectListItem> Dokters { get; set; }
        public IEnumerable<SelectListItem> Afspraaktypes { get; set; }

        public int GeselecteerdeAfspraakType { get; set; }

        public int GeselecteerdeDokter { get; set; }
    }

    public class CreateAfspraakValidator: AbstractValidator<CreateAfspraakViewModel>
    {
        public CreateAfspraakValidator()
        {
            RuleFor(x => x.GeselecteerdeDokter).NotEmpty().WithMessage("Selecteer een dokter!");
            RuleFor(x => x.GeselecteerdeAfspraakType).NotEmpty().WithMessage("Selecteer een type afspraak!");

        }
    }
    public class ValidateKlacht : AbstractValidator<Klacht>
    {
        public ValidateKlacht()
        {
            RuleFor(x => x.Omschrijving).NotNull().WithMessage("Mag niet leeg zijn");
        }
    }
}
