using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BookFlix.Infrastructure.Data
{
    internal class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            var basePath = Directory.GetCurrentDirectory();
            if (!File.Exists(Path.Combine(basePath, "appsettings.json")))
            {
                basePath = Path.GetFullPath(Path.Combine(basePath, "../BookFlix.Web"));
            }
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var constr = configuration.GetConnectionString("DefaultConnection");
            AppDbContextConfiguration.Configure(optionsBuilder, constr);

            return new AppDbContext(optionsBuilder.Options);
        }
    }

}
