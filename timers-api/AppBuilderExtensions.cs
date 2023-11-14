using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using timers_api.persistence;
using timers_api.domain;
using timers_api;
using timers_api.Middleware;

namespace timers_api;

public static class AppBuilderExtensions
{
    [DebuggerStepThrough]
    public static void AddAppServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddDomainServices(builder.Configuration);
        //builder.Services.AddInfrastructure();
        builder.Services.AddPersistenceServices(builder.Configuration);
    }

    public static void AddWebApiInfrastructure(this WebApplicationBuilder builder, bool addSwagger = true)
    {
        builder.Services.AddControllers();

        if (addSwagger)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }

        builder.Services.AddLogging();
    }

    public static string AddCorsAllowAnyOrigin(this WebApplicationBuilder builder, string policyName = "AllowAnyOrigin")
    {
        builder.Services.AddCors(x => x.AddPolicy(policyName, b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().Build()));
        return policyName;
    }

    private static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connStr = configuration.GetConnectionString("DefaultConnection");
        if (connStr == null)
            throw new Exception("Connection string `DefaultConnection` is missing. Check appsettings.");

        services.AddPersistenceServices(connStr, "timers-api.persistence");
    }
}

public static class WebApplicationExtensions
{
    public static void ConfigureAppServices(this WebApplication app, string corsPolicy)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Timers API v1");
            });
        }

        app.UseMigrations();
        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseHttpsRedirection();
        app.UseCors(corsPolicy);
        app.UseAuthorization();
        app.MapControllers();
    }

    private static void UseMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        EnsureDatabaseCreated(context);
    }

    private static void EnsureDatabaseCreated(DataContext context)
    {
        // run Migrations
        context.Database.Migrate();
    }
}
