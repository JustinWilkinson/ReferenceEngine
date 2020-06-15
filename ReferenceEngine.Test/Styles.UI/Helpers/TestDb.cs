using Microsoft.EntityFrameworkCore;
using ReferenceEngine.Styles.UI.Database;

namespace ReferenceEngine.Test.Styles.UI.Helpers
{
    public class TestDb : ApplicationDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("Test");
        }
    }
}