using Dokterspunt.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.ViewModels
{
    public class MedischDossierViewModel
    {
        public Patiënt Patiënt { get; set; }

        public MedischDossier Dossier { get; set; }

        public string Verleden { get; set; }
    }


    public class DossierValidator: AbstractValidator<MedischDossierViewModel>
    {
        public DossierValidator()
        {
            RuleFor(x => x.Verleden).NotNull().WithMessage("Er moet een medisch verleden aanwezig zijn!");
        }
    }
}
