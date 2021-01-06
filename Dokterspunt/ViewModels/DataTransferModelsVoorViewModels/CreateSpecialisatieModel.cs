using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.ViewModels.ModelsForViewModels
{
    public class CreateSpecialisatieModel
    {
        public string Naam { get; set; }
        
        public string Beschrijving { get; set; }
    }

    public class CreateSpecialisatieValidator : AbstractValidator<CreateSpecialisatieModel>
    {
        public CreateSpecialisatieValidator()
        {
            RuleFor(x => x.Naam).NotNull().WithMessage("Er moet een naam zijn ingevuld");
           
            RuleFor(x => x.Beschrijving).NotNull().WithMessage("Er moet een beschrijving zijn ingevuld");
        }
    }
}
