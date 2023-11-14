using timers_api.domain.Data.Repositories;
using timers_api.domain.Entities;

namespace timers_api.persistence.Repositories
{
    public class TimersRepository : Repository<TimerEntity>, ITimersRepository
    {
        public TimersRepository(DataContext context) : base(context)
        {
        }
    }
}
