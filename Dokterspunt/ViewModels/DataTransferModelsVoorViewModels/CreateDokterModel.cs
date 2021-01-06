using Dokterspunt.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.ViewModels
{
    public class CreateDokterModel
    {
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public IEnumerable<SelectListItem> Praktijken { get; set; }

        public IEnumerable<SelectListItem> Specialiteiten { get; set; }

        public int GeselecteerdePraktijk { get; set; }
        public int GeselecteerdeSpecialiteit { get; set; }
        public string Email { get; set; }
        public string Wachtwoord { get; set; }

        public string Herhaal { get; set; }
    }

    public class CreateDokterValidatior: AbstractValidator<CreateDokterModel>
    {
        public CreateDokterValidatior()
        {
            RuleFor(x => x.Voornaam).NotEmpty().WithMessage("Er moet een voornaam zijn ingevuld!");
            RuleFor(x => x.Achternaam).NotEmpty().WithMessage("Er moet een achternaam zijn ingevuld!");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Dit address is niet valide");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Dit veld moet worden ingevuld!");
            RuleFor(x => x.GeselecteerdePraktijk).NotEmpty().WithMessage("Er moet een praktijk zijn geselecteerd!");
            RuleFor(x => x.GeselecteerdeSpecialiteit).NotEmpty().WithMessage("Er moet een specialiteit zijn geselecteerd!");
            RuleFor(x => x.Wachtwoord).NotEmpty().WithMessage("Het veld wachtwoord is vereist!");
            RuleFor(x => x.Wachtwoord).MinimumLength(8).WithMessage("Het wachtwoord moet minstens 8 karakters lang zijn!");
            RuleFor(x => x.Herhaal).NotEmpty().WithMessage("Het veld wachtwoord is vereist!");
            RuleFor(x => x.Herhaal).Equal(z => z.Wachtwoord).WithMessage("De wachtwoorden komen niet overeen!");
        }
    }
}
