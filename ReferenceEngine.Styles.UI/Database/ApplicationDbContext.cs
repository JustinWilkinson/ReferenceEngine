using Bibtex.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace LatexReferences.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<EntryStyle> EntryStyles { get; set; }

        public DbSet<BibliographyStyle> BibliographyStyles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=LatexReferences.sqlite");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EntryStyle>().Property(es => es.FieldsJson).HasColumnName("Fields");
        }
    }
}