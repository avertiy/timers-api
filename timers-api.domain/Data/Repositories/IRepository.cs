using System.Linq.Expressions;

namespace timers_api.domain.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        ValueTask<T?> FindAsync(object id);
        ValueTask<T?> FindByGuid(Guid id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
    }
}
