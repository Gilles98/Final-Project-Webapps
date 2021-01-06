using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.ViewModels
{
    public class UpdateDokterModel
    {
        public string Voornaam { get; set; }

        public string Achternaam { get; set; }
        public IEnumerable<SelectListItem> Praktijken { get; set; }

        public IEnumerable<SelectListItem> Specialiteiten { get; set; }

        public string Email { get; set; }

        public int? GeselecteerdePraktijk { get; set; }


        public int? GeselecteerdeSpecialiteit { get; set; }
        public int GeselecteerdeDokter { get; set; }

        public string Wachtwoord { get; set; }


        public string Herhaal { get; set; }

        
        public IEnumerable<SelectListItem> Dokters { get; set; }
    }

    public class UpdateDokterValidator: AbstractValidator<UpdateDokterModel>
    {
        public UpdateDokterValidator()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("Dit is geen geldig e-mailadres");
            RuleFor(x => x.GeselecteerdeDokter).NotEmpty().WithMessage("Er moet een dokter worden geselecteerd");
            RuleFor(x => x.Herhaal).Equal(x => x.Wachtwoord).WithMessage("De wachtwoorden komen niet overeen!");
            RuleFor(x => x.Wachtwoord).MinimumLength(8).WithMessage("Het wachtwoord moet minstens 8 karakters bevatten");
        }
    }
}
