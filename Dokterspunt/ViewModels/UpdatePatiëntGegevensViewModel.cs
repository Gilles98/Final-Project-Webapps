using Dokterspunt.Areas.Identity.Data;
using Dokterspunt.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.ViewModels
{
    public class UpdatePatiëntGegevensViewModel
    {
        public  Patiënt Patiënt { get; set; }

        public string Wachtwoord { get; set; }

        public string Email { get; set; }
    }

    public class UpdatePatiëntGegevensValidator: AbstractValidator<UpdatePatiëntGegevensViewModel>
    {
        public UpdatePatiëntGegevensValidator()
        {
            RuleFor(x => x.Patiënt.Voornaam).NotNull().WithMessage("Voornaam mag niet leeg zijn");
            RuleFor(x => x.Patiënt.Achternaam).NotNull().WithMessage("Achternaam mag niet leeg zijn");
            RuleFor(x => x.Patiënt.Gemeente).NotNull().WithMessage("Gemeente mag niet leeg zijn");
            RuleFor(x => x.Patiënt.Straat).NotNull().WithMessage("Straat mag niet leeg zijn");
            RuleFor(x => x.Patiënt.HuisNr).NotNull().WithMessage("Huisnummer mag niet leeg zijn");
            RuleFor(x => x.Patiënt.Postcode).NotNull().WithMessage("Postcode mag niet leeg zijn");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Dit e-mailadres is niet valide!");
        }
    }
}
