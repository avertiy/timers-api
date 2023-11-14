using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using timers_api.domain.Data.Repositories;

namespace timers_api.persistence.Repositories
{
    /// <summary>
    /// The repository (and unit of work) patterns are intended to create an abstraction layer between the data access layer and the business logic layer. 
    /// Implementing these patterns can help insulate your application from changes in the data store and can facilitate automated unit testing or TDD.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DataContext _dbContext;

        private DbSet<T>? _dbSet;

        protected DbSet<T> DbSet
        {
            get => _dbSet ??= _dbContext.Set<T>();
        }

        public Repository(DataContext context)
        {
            _dbContext = context;
        }

        public virtual void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return DbSet.Where(expression);
        }

        public virtual void Update(T entity)
        {
            DbSet.Update(entity);
        }

        public virtual ValueTask<T?> FindAsync(object id)
        {
            return DbSet.FindAsync(id);
        }

        public ValueTask<T?> FindByGuid(Guid id)
        {
            return DbSet.FindAsync(id);
        }
    }
}
