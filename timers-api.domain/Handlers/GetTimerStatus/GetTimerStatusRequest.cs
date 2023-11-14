using FluentValidation;

namespace timers_api.domain.Handlers.GetTimerStatus
{
    public class GetTimerStatusRequest
    {
        public Guid Id { get; set; }
    }

    public class GetTimerStatusRequestValidator : AbstractValidator<GetTimerStatusRequest>
    {
        public GetTimerStatusRequestValidator()
        {
            RuleFor(x=> x.Id).NotEmpty();
        }
    }

    public class GetTimerStatusResponse
    {
        public Guid? Id { get; set; }
        public int TimeLeft { get; set; }
    }
}