using FluentValidation;
using System.ComponentModel;

namespace timers_api.domain.Handlers.SetTimer
{
    public class SetTimerRequest
    {
        [DefaultValue("0")]
        public int Hours { get; set; } = 0;
        [DefaultValue("0")]
        public int Minutes { get; set; } = 0;
        [DefaultValue("30")]
        public int Seconds { get; set; } = 0;
        [DefaultValue("https://google.com/")]
        public string WebhookUrl { get; set; } = null!;
    }

    public class SetTimerRequestValidator : AbstractValidator<SetTimerRequest>
    {
        public SetTimerRequestValidator()
        {
            RuleFor(x => x.Hours).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Minutes).GreaterThanOrEqualTo(0).LessThan(60);
            RuleFor(x => x.Seconds).GreaterThanOrEqualTo(0).LessThan(60);
            RuleFor(x => x.WebhookUrl)
            .NotEmpty().WithMessage("URL cannot be empty.")
            .Must(IsValidUrl).WithMessage("Invalid URL format.");
        }

        private bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }

    public class SetTimerResponse
    {
        public Guid Id { get; set; }
    }
}