using LatexReferences.Models.Format;
using Microsoft.EntityFrameworkCore;

namespace LatexReferences.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<EntryStyle> EntryStyles { get; set; }

        public DbSet<BibliographyStyle> BibliographyStyles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=LatexReferences.sqlite");
        }
    }
}