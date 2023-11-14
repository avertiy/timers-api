using timers_api.domain.Services;

namespace AVS.MultiExchange.DataLoaderApi;

public class HostedService : BackgroundService
{
    private readonly ILogger<HostedService> _logger;
    private IServiceProvider _sp;

    private readonly PeriodicTimer _timer;

    public HostedService(IServiceProvider sp, ILogger<HostedService> logger) { 
        _sp = sp; 
        _logger = logger;
        // let's say every 300 seconds background task will check timers and schedule webhook execution
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(TimersService.PROCESS_TIMERS_INTERVAL)); }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        _logger.LogInformation("Starting timers-api HostedService.. [interval: {Interval} seconds]", TimersService.PROCESS_TIMERS_INTERVAL);

        var nextTick = true;
        while (nextTick)
        {
            using (var scope = _sp.CreateScope())
            {
                await ProcessTimers(scope.ServiceProvider, ct);
            }

            nextTick = await _timer.WaitForNextTickAsync();
        }

        _logger.LogInformation("Hosted Service started.");
    }

    private async Task ProcessTimers(IServiceProvider sp, CancellationToken ct)
    {
        try
        {
            var timerService = sp.GetRequiredService<ITimersService>();
            var count = await timerService.ProcessTimers(ct);
            _logger.LogInformation("Executed #{count} timers", count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Load data failed");
        }
    }
}

