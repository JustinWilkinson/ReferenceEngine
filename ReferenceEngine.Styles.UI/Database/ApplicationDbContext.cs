using Microsoft.EntityFrameworkCore;
using ReferenceEngine.Bibtex.Abstractions;

namespace ReferenceEngine.Styles.UI.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<EntryStyle> EntryStyles { get; set; }

        public DbSet<BibliographyStyle> BibliographyStyles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=ReferenceEngineStyles.sqlite");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EntryStyle>().Property(es => es.FieldsJson).HasColumnName("Fields");
        }
    }
}