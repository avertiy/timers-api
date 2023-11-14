using Microsoft.Extensions.Logging;
using timers_api.domain.Data;
using timers_api.domain.Data.DataServices;
using timers_api.domain.Entities;

namespace timers_api.domain.Services;

public interface ITimersService
{
    Task<int> ProcessTimers(CancellationToken ct);
}
public class TimersService : ITimersService
{
    private readonly IDateTimeProvider _datetimeProvider;
    private readonly ITimersDataService _dataService;
    private readonly ILogger<TimersService> _logger;

    public const int PROCESS_TIMERS_INTERVAL = 300;

    public TimersService(IDateTimeProvider datetimeProvider, ITimersDataService dataService, ILogger<TimersService> logger)
    {
        _datetimeProvider = datetimeProvider;
        _dataService = dataService;
        _logger = logger;
    }

    public async Task<int> ProcessTimers(CancellationToken ct)
    {
        TimerEntity[] timers = await _dataService.GetActiveTimers(ct);
        var tasks = new List<Task>(timers.Length);
        foreach (var timer in timers)
        {
            var serverTime = _datetimeProvider.GetSystemTime();
            var timeLeft = timer.CalculateTimeLeft(serverTime);

            if (timeLeft < 0)
            {
                _logger.LogWarning("Timer #{id} has expired and is executed with a delay {delay} sec.", timer.Id, timeLeft);
                tasks.Add(Task.Run(() => ExecuteWebHook(timer, ct)));
                continue;
            }

            if(timeLeft >=0 && timeLeft <= PROCESS_TIMERS_INTERVAL)
            {
                tasks.Add(Task.Run(async () => await ExecuteWebHook(timer, delay: timeLeft * 1000, ct)));
            }
        }

        await Task.WhenAll(tasks);
        return tasks.Count;
    }

    private void ExecuteWebHook(TimerEntity timer, CancellationToken ct)
    {
        _logger.LogWarning("WebHook `{url}` executed", timer.WebhookUrl);
        timer.Status = TimerStatus.Finished;
        _dataService.UpdateTimer(timer);
    }

    private async Task ExecuteWebHook(TimerEntity timer, int delay, CancellationToken ct)
    {
        await Task.Delay(delay);
        _logger.LogWarning("WebHook `{url}` executed", timer.WebhookUrl);
        timer.Status = TimerStatus.Finished;
        _dataService.UpdateTimer(timer);
    }
}