using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using timers_api.domain.Data;
using timers_api.domain.Data.Repositories;
using timers_api.persistence.Repositories;

namespace timers_api.persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        private ITimersRepository? _timers;

        public ITimersRepository Timers => _timers ??= new TimersRepository(_context);

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public Task<int> CommitAsync(CancellationToken ct = default)
        {   
            return _context.SaveChangesAsync(ct);
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public void Rollback()
        {
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }
    }
}
