using Dokterspunt.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dokterspunt.Areas.Identity.Data
{
    public class LoggedInUser : IdentityUser
    {
        [PersonalData]
        public Dokter Dokter { get; set; }

        [PersonalData]
        public Patiënt Patient { get; set; }
    }
}
