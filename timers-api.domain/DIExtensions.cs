using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using timers_api.domain.Data;
using timers_api.domain.Data.DataServices;
using timers_api.domain.Handlers.GetTimerStatus;
using timers_api.domain.Handlers.SetTimer;
using timers_api.domain.Services;

namespace timers_api.domain;

public static class DIExtensions
{
    public static void AddDomainServices(this IServiceCollection services, IConfiguration configuration)
    {   
        services.AddRequestValidators();
        services.AddRequestHandlers();
        services.AddDataServices();
        services.AddScoped<ITimersService, TimersService>();
    }

    private static void AddRequestHandlers(this IServiceCollection services)
    {
        services.AddScoped<SetTimerRequestHandler>();
        services.AddScoped<GetTimerStatusRequestHandler>();
    }

    private static void AddRequestValidators(this IServiceCollection services)
    {
        //original property name in the error, i.e. without words separation with space 
        FluentValidation.ValidatorOptions.Global.DisplayNameResolver = (type, member, expression) => member?.Name;
        services.AddScoped<SetTimerRequestValidator>();
        services.AddScoped<GetTimerStatusRequestValidator>();
    }

    private static void AddDataServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<ITimersDataService, TimersDataService>();
    }
}
