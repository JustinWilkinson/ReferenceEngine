﻿using Microsoft.EntityFrameworkCore;

namespace Bibtex.Abstractions
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<EntryStyle> EntryStyles { get; set; }

        public DbSet<BibliographyStyle> BibliographyStyles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=LatexReferences.sqlite", b => b.MigrationsAssembly("Bibtex"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EntryStyle>().Property(es => es.FieldsString).HasColumnName("Fields");
        }
    }
}