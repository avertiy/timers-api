using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using timers_api.domain.Data;

namespace timers_api.persistence
{
    [ExcludeFromCodeCoverage]
    public static class DIExtensions
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
            string connectionStr, string migrationsAssemblyName)
        {
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(connectionStr,
                    sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssemblyName)));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
