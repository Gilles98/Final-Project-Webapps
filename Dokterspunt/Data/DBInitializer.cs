using System;
using System.Collections.Generic;
using System.Linq;
using Dokterspunt.Areas.Identity.Data;
using Dokterspunt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dokterspunt.Data
{
    public static class DBInitializer
    {
        public static void Initializer(DokterspuntContext context)
        {
            context.Database.EnsureCreated();



            ///vanwege het te laat doorhebben dat ik deze value van te voren moest seeden in de database
            if (!context.AfspraakTypes.Any())
            {
                context.AfspraakTypes.AddRange(new AfspraakType() { SoortAfspraak = "Huisbezoek" }, new AfspraakType() { SoortAfspraak = "In de praktijk" });
                context.SaveChanges();
            }

            if (context.Dokters.Any())
            {
                return;
            }

            /// <summary>
            /// configureer de admin voor het initializen
            /// </summary>

            context.Specialisaties.AddRange(new Specialisatie
            { Omschrijving = "Huisarts" },
            new Specialisatie { Omschrijving = "Dermatoloog" },
            new Specialisatie { Omschrijving = "Gynaecoloog" },
            new Specialisatie { Omschrijving = "Cardioloog" }
            );
            context.Praktijken.AddRange(new Praktijk { Breedtegraad = (Decimal)51.161073, Lengtegraad = (Decimal)4.961094, Gemeente = "Geel", HuisNr = "4", Postcode = 2440, Straat = "Kleinhoefstraat" });
            context.Praktijken.AddRange(new Praktijk { Breedtegraad = (Decimal)51.085398, Lengtegraad = (Decimal)4.670883, Gemeente = "Berlaar", HuisNr = "36", Postcode = 2590, Straat = "Melkouwensteenweg" });

            ///vanwege ID check
            context.SaveChanges();

            LoggedInUser user = new LoggedInUser();
            user.UserName = "Main dokter";
            user.PhoneNumber = "0456545465";
            PasswordHasher<LoggedInUser> password = new PasswordHasher<LoggedInUser>();
            user.PasswordHash = password.HashPassword(user, "Ditiseentest1!");
            user.Email = "guigilles@gmail.com";
            user.EmailConfirmed = true;
            user.NormalizedEmail = "guigilles@gmail.com";
            user.NormalizedUserName = user.UserName.ToUpper();



            Dokter dokter = new Dokter() { UserID = user.Id, LoggedUser = user, Voornaam = "Gilles", Achternaam = "Gui", PraktijkID = 1, SpecialisatieID = 1 };
            user.Dokter = dokter;
            context.Users.Add(user);
            context.Dokters.Add(dokter);

            //admin instellen die ook dokter is

            DbSet<IdentityUserRole<string>> roles = context.UserRoles;
            IdentityRole role = context.Roles.FirstOrDefault(x => x.Name == "Admin");
            if (!roles.Any(us => us.UserId == user.Id && us.RoleId == role.Id))
            {
                roles.Add(new IdentityUserRole<string>() { UserId = user.Id, RoleId = role.Id });
            }

            role = context.Roles.FirstOrDefault(x => x.Name == "Dokter");
            if (!roles.Any(us => us.UserId == user.Id && us.RoleId == role.Id))
            {
                roles.Add(new IdentityUserRole<string>() { UserId = user.Id, RoleId = role.Id });
            }
            context.SaveChanges();
        }
    }
}
