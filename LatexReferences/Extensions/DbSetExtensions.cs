using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LatexReferences.Extensions
{
    public static class DbSetExtensions
    {
        public static async Task<(bool Found, T Value)> TryFindAsync<T>(this DbSet<T> db, params object[] keyValues) where T : class
        {
            var result = await db.FindAsync(keyValues);
            return (result != null, result);
        }
    }
}