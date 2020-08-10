using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Succubus.Database.Context;
using Succubus.Database.Repositories.Interfaces;

namespace Succubus.Database.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly SuccubusContext Context;
        protected DbSet<T> Set;

        public Repository(SuccubusContext context)
        {
            Context = context;
            Set = Context.Set<T>();
        }

        public T Get(Guid id)
        {
            return Set.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return Set.ToList();
        }

        public void Add(T entity)
        {
            Set.Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            await Set.AddAsync(entity).ConfigureAwait(false);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            Set.AddRange(entities);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await Set.AddRangeAsync(entities).ConfigureAwait(false);
        }

        public void Remove(T entity)
        {
            Set.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            Set.RemoveRange(entities);
        }
    }
}