using System;
using System.Collections.Generic;
using System.Text;
using Dokterspunt.Areas.Identity.Data;
using Dokterspunt.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dokterspunt.Data
{
    public class DokterspuntContext : IdentityDbContext<LoggedInUser>
    {
        public DokterspuntContext() { }
        public DokterspuntContext(DbContextOptions<DokterspuntContext> options)
            : base(options)
        {
        }
        public DbSet<Afspraak>Afspraken { get; set; }
        public DbSet<AfspraakType> AfspraakTypes { get; set; }

        public DbSet<Dokter>Dokters { get; set; }
        public DbSet<Klacht>Klachten { get; set; }
        public DbSet<KlachtPatiënt> KlachtenPatiënten { get; set; }
        public DbSet<MedischDossier>MedischeDossiers { get; set; }
        public DbSet<Patiënt>Patiënten { get; set; }
        public DbSet<Praktijk>Praktijken { get; set; }
        public DbSet<Specialisatie>Specialisaties { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Webapplicaties");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Afspraak>().ToTable("Afspraak");
            modelBuilder.Entity<AfspraakType>().ToTable("Afspraaktype");
            modelBuilder.Entity<Dokter>().ToTable("Dokter");
            modelBuilder.Entity<Klacht>().ToTable("Klacht");
            modelBuilder.Entity<KlachtPatiënt>().ToTable("KlachtPatiënt");
            modelBuilder.Entity<MedischDossier>().ToTable("MedischDossier");
            modelBuilder.Entity<Patiënt>().ToTable("Patiënt");
            modelBuilder.Entity<Praktijk>().ToTable("Praktijk");
            modelBuilder.Entity<Specialisatie>().ToTable("Specialisatie");
            modelBuilder.Entity<Patiënt>().Property(p => p.HuisNr).HasMaxLength(20);
            ///op erd staat 255 maar dit was verkeerd
            modelBuilder.Entity<Praktijk>().Property(p => p.HuisNr).HasMaxLength(20);
            modelBuilder.Entity<Praktijk>().Property(p => p.Lengtegraad).HasColumnType("decimal(30,20)");
            modelBuilder.Entity<Praktijk>().Property(p => p.Breedtegraad).HasColumnType("decimal(30,20)");
            modelBuilder.Entity<Praktijk>().Ignore(x => x.VolledigAdress);
            modelBuilder.Entity<Dokter>().Ignore(x => x.VolledigeGegevens);
        }
    }
}
