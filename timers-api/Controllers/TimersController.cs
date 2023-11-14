using Microsoft.AspNetCore.Mvc;
using timers_api.domain.Handlers.GetTimerStatus;
using timers_api.domain.Handlers.SetTimer;

namespace timers_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimersController : ControllerBase
    {
        private readonly ILogger<TimersController> _logger;
        private readonly IServiceProvider _serviceProvider;

        public TimersController(ILogger<TimersController> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        [HttpPost("set")]
        public async Task<SetTimerResponse> SetTimer([FromQuery] SetTimerRequest request, CancellationToken ct)
        {
            var handler = _serviceProvider.GetRequiredService<SetTimerRequestHandler>();
            var response = await handler.Handle(request, ct);
            return response;
        }

        [HttpGet("status/{id}")]
        public async Task<IActionResult> GetTimerStatus([FromQuery] GetTimerStatusRequest request, CancellationToken ct)
        {
            var handler = _serviceProvider.GetRequiredService<GetTimerStatusRequestHandler>();
            var response = await handler.Handle(request, ct);
            return Ok(response);
        }        
    }
}