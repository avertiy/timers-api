using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace timers_api.persistence
{
    /// <summary>
    /// DataContextFactory is needed for just adding/running migrations 
    /// it is due to PMC and dotnet-ef tools are not yet upgraded to deal with a new WebApplication.CreateBuilder(args) approach
    /// </summary>
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            var connStr = "Server=localhost;Database=timers;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
            optionsBuilder.UseSqlServer(connStr, sqlOptions => sqlOptions.MigrationsAssembly("timers-api.persistence"));
            return new DataContext(optionsBuilder.Options);
        }
    }
}
