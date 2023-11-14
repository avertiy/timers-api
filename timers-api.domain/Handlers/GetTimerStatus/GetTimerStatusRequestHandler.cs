using FluentValidation;
using timers_api.domain.Data;
using timers_api.domain.Data.DataServices;

namespace timers_api.domain.Handlers.GetTimerStatus;

public class GetTimerStatusRequestHandler
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ITimersDataService _timersDataService;
    private readonly GetTimerStatusRequestValidator _validator;

    public GetTimerStatusRequestHandler(IDateTimeProvider dateTimeProvider, ITimersDataService timersDataService, GetTimerStatusRequestValidator validator)
    {
        _dateTimeProvider = dateTimeProvider;
        _timersDataService = timersDataService;
        _validator = validator;
    }

    public async Task<GetTimerStatusResponse> Handle(GetTimerStatusRequest request, CancellationToken ct)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
            throw new ValidationException($"{nameof(GetTimerStatusRequest)} is invalid", validationResult.Errors);

        var (id, timeLeft) = await ProcessRequest(request, ct);

        var response = new GetTimerStatusResponse() { Id = id, TimeLeft = timeLeft };
        return response;
    }

    private async Task<(Guid id, int timeLeft)> ProcessRequest(GetTimerStatusRequest request, CancellationToken ct)
    {
        var timer = await _timersDataService.GetTimer(request.Id, ct);

        if(timer == null)
            throw new NotFoundException($"Timer #`{request.Id}` not found");

        var timeLeft = timer.CalculateTimeLeft(_dateTimeProvider.GetSystemTime());
        return (timer.Id, timeLeft);
    }
}
