using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.ViewModels.ModelsForViewModels
{
    public class UpdateSpecialisatieModel
    {

        public string Beschrijving { get; set; }

        public IEnumerable<SelectListItem> Specialisaties { get; set; }

        public int GeselecteerdeSpecialisatie { get; set; }
    }

    public class UpdateSpecialisatieValidator : AbstractValidator<UpdateSpecialisatieModel>
    {
        public UpdateSpecialisatieValidator()
        {
            RuleFor(x => x.GeselecteerdeSpecialisatie).NotEmpty().WithMessage("Er moet een specialisatie zijn geselecteerd!");
        }
    }
}

