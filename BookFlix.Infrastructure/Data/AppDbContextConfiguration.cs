using Microsoft.EntityFrameworkCore;

namespace BookFlix.Infrastructure.Data
{
    internal static class AppDbContextConfiguration
    {
        public static void Configure(DbContextOptionsBuilder<AppDbContext> optionsBuilder, string constr)
        {
            optionsBuilder.UseSqlServer(constr);
        }
    }
}
