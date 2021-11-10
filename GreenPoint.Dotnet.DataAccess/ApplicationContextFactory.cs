using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace GreenPoint.Dotnet.DataAccess
{
    public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            // Build config
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.core.json")
                .Build();

            var connectionString = config.GetConnectionString("DevConnection");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationContext(optionsBuilder.Options);
        }
    }

}
