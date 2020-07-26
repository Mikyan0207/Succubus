using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Succubus.Database.Repositories
{
    public interface IRepository<T> where T : class
    {
        T Get(Guid id);

        IEnumerable<T> GetAll();

        void Add(T entity);

        Task AddAsync(T entity);

        void AddRange(IEnumerable<T> entities);

        Task AddRangeAsync(IEnumerable<T> entities);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);

        IEnumerable<T> Where(Expression<Func<T, bool>> predicate);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}
