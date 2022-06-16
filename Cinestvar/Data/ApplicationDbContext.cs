using Cinestvar.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinestvar.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Film> Film { get; set; }
        public DbSet<Rezervacija> Rezervacija { get; set; }
        public DbSet<RezervacijaSjedista> RezervacijaSjedista { get; set; }
        public DbSet<Sala> Sala { get; set; }
        public DbSet<Stavka> Stavka { get; set; }
        public DbSet<Termin> Termin { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Film>().ToTable("Film");
            modelBuilder.Entity<Rezervacija>().ToTable("Rezervacija");
            modelBuilder.Entity<RezervacijaSjedista>().ToTable("RezervacijaSjedista");
            modelBuilder.Entity<Sala>().ToTable("Sala");
            modelBuilder.Entity<Stavka>().ToTable("Stavka");
            modelBuilder.Entity<Termin>().ToTable("Termin");
            base.OnModelCreating(modelBuilder);
        }
    }
}
