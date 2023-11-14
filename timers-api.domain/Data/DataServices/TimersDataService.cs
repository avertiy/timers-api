using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using timers_api.domain.Data.Repositories;
using timers_api.domain.Entities;

namespace timers_api.domain.Data.DataServices
{
    public interface ITimersDataService
    {
        Task AddTimer(TimerEntity timer, CancellationToken ct);
        Task<TimerEntity[]> GetActiveTimers(CancellationToken ct);
        Task<TimerEntity?> GetTimer(Guid id, CancellationToken ct);
        void UpdateTimer(TimerEntity timer);
    }

    public class TimersDataService : ITimersDataService
    {
        private IUnitOfWork _unitOfWork;
        private ITimersRepository _repository;
        private ILogger<TimersDataService> _logger;

        public TimersDataService(IUnitOfWork unitOfWork, ILogger<TimersDataService> logger)
        {
            _unitOfWork = unitOfWork;
            _repository = unitOfWork.Timers;
            _logger = logger;
        }

        public async Task<TimerEntity?> GetTimer(Guid id, CancellationToken ct)
        {
            var timer = await _repository.FindByGuid(id);
            return timer;
        }       

        public async Task AddTimer(TimerEntity timer, CancellationToken ct)
        {
            try
            {
                _repository.Add(timer);
                var count = await _unitOfWork.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(AddTimer)} failed");
                throw;
            }
        }

        public async Task<TimerEntity[]> GetActiveTimers(CancellationToken ct)
        {
            var query = _repository.Where(x => x.Status == TimerStatus.Started);
            return await query.ToArrayAsync(ct);
        }

        public void UpdateTimer(TimerEntity timer)
        {
            try
            {
                lock (_repository)
                {
                    _repository.Update(timer);
                    _unitOfWork.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(AddTimer)} failed");
                throw;
            }
        }
    }
}