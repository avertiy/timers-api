using timers_api.domain.Data.Repositories;

namespace timers_api.domain.Data
{
    public interface IUnitOfWork
    {
        int Commit();
        Task<int> CommitAsync(CancellationToken ct = default);
        void Rollback();
        ITimersRepository Timers { get; }
    }
}
