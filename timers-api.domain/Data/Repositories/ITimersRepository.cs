using timers_api.domain.Entities;

namespace timers_api.domain.Data.Repositories
{
    public interface ITimersRepository : IRepository<TimerEntity>
    {
        //new void Add(TimerEntity entity);
    }
}
