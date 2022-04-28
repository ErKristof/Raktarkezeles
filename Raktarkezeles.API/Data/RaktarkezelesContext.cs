using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Raktarkezeles.API.Models;

#nullable disable

namespace Raktarkezeles.API.Data
{
    public partial class RaktarkezelesContext : DbContext
    {
        public RaktarkezelesContext()
        {
        }

        public RaktarkezelesContext(DbContextOptions<RaktarkezelesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Alkatresz> Alkatreszek { get; set; }
        public virtual DbSet<AlkatreszElofordulas> AlkatreszElofordulasok { get; set; }
        public virtual DbSet<Gyarto> Gyartok { get; set; }
        public virtual DbSet<Kategoria> Kategoriak { get; set; }
        public virtual DbSet<MennyisegiEgyseg> MennyisegiEgysegek { get; set; }
        public virtual DbSet<RaktarozasiHely> RaktarozasiHelyek { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=RaktarTeszt");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RaktarozasiHely>().ToTable("RaktarozasiHely");
            modelBuilder.Entity<MennyisegiEgyseg>().ToTable("MennyisegiEgyseg");
            modelBuilder.Entity<Kategoria>().ToTable("Kategoria");
            modelBuilder.Entity<Gyarto>().ToTable("Gyarto");
            modelBuilder.Entity<AlkatreszElofordulas>().ToTable("AlkatreszElofordulas");
            modelBuilder.Entity<Alkatresz>().ToTable("Alkatresz");
        }
    }
}
