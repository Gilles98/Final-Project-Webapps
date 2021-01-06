using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.ViewModels.ModelsForViewModels
{
    public class CreatePraktijkModel
    {
        public string Gemeente { get; set; }
      
        public string Straat { get; set; }
        public string HuisNr { get; set; }
        public int Postcode { get; set; }

        public Decimal Lengtegraad { get; set; }

        public Decimal Breedtegraad { get; set; }
    }

    public class CreatePraktijkValidator: AbstractValidator<CreatePraktijkModel>
    {
        public CreatePraktijkValidator()
        {
            RuleFor(x => x.Gemeente).NotNull().WithMessage("Er moet een gemeente zijn ingevuld");
            RuleFor(x => x.Breedtegraad).NotNull().WithMessage("Er moet een breedtegraad decimaal zijn meegegeven");
            RuleFor(x => x.Breedtegraad).ScalePrecision(12, 30).WithMessage("Deze breedtegraad voldoet niet aan de eisen!");
            RuleFor(x => x.Lengtegraad).NotNull().WithMessage("Er moet een lengtegraad decimaal zijn meegegeven");
            RuleFor(x => x.HuisNr).NotNull().WithMessage("Er moet een huisnummer zijn ingevuld");
            RuleFor(x => x.Straat).NotNull().WithMessage("Er moet een straat zijn ingevuld");
            RuleFor(x => x.Postcode).InclusiveBetween(1000, 9999).WithMessage("Postcode moet tussen de 1000 en 9999 liggen");
        }
    }
}
