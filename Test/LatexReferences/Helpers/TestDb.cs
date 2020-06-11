using Bibtex.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Test.LatexReferences.Helpers
{
    public class TestDb : ApplicationDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("Test");
        }
    }
}