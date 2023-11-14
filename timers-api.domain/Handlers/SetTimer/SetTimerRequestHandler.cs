using FluentValidation;
using timers_api.domain.Data;
using timers_api.domain.Data.DataServices;
using timers_api.domain.Entities;

namespace timers_api.domain.Handlers.SetTimer;

public class SetTimerRequestHandler
{
    private readonly ITimersDataService _timersDataService;
    private readonly SetTimerRequestValidator _validator;
    private readonly IDateTimeProvider _dateTimeProvider;

    public SetTimerRequestHandler(ITimersDataService timersDataService, SetTimerRequestValidator validator, IDateTimeProvider dateTimeProvider)
    {
        _timersDataService = timersDataService;
        _validator = validator;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<SetTimerResponse> Handle(SetTimerRequest request, CancellationToken ct)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
            throw new ValidationException($"{nameof(SetTimerRequest)} is invalid", validationResult.Errors);

        var id = await ProcessRequest(request, ct);

        var response = new SetTimerResponse() { Id = id };
        return response;
    }

    private async Task<Guid> ProcessRequest(SetTimerRequest request, CancellationToken ct)
    {
        var timer = new TimerEntity()
        {
            Id = Guid.NewGuid(),
            Hours = request.Hours,
            Minutes = request.Minutes,
            Seconds = request.Seconds,
            WebhookUrl = request.WebhookUrl,
            Status = TimerStatus.Started,
            Timestamp = _dateTimeProvider.GetSystemTime() //DateTime.Now
        };

        timer.UpdateStatus(_dateTimeProvider.GetSystemTime());

        await _timersDataService.AddTimer(timer, ct);
        return timer.Id;
    }

}
