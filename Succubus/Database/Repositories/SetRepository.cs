using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NLog;
using Succubus.Commands.Nsfw.Options;
using Succubus.Database.Context;
using Succubus.Database.Extensions;
using Succubus.Database.Models;
using Succubus.Database.Repositories.Interfaces;

namespace Succubus.Database.Repositories
{
    public class SetRepository : Repository<Set>, ISetRepository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public SetRepository(SuccubusContext context) : base(context)
        {
        }

        public async Task<Set> GetSetAsync(YabaiOptions options)
        {
            return Context.Sets
                .Include(x => x.Cosplayer)
                .ToList()
                .Where(x => options.SafeMode ? x.YabaiLevel == YabaiLevel.Safe : x.YabaiLevel >= YabaiLevel.Safe)
                .ConditionalWhere(options.Set != null, x =>
                    x.Name.ToLowerInvariant().LevenshteinDistance(options.Set.ToLowerInvariant()) < 2
                    || x.Aliases.Any(y => y.ToLowerInvariant().LevenshteinDistance(options.Set.ToLowerInvariant()) < 2))
                .ConditionalWhere(options.User != null, x =>
                    x.Cosplayer.Name.ToLowerInvariant().LevenshteinDistance(options.User) < 2
                    || x.Cosplayer.Aliases.Any(y => y.ToLowerInvariant().LevenshteinDistance(options.User) < 2))
                .OrderBy(x => new Random().Next())
                .Take(1)
                .FirstOrDefault();
        }

        public async Task<Set> GetSetAsync(string name)
        {
            return await Context.Sets
                .Include(x => x.Cosplayer)
                .AsAsyncEnumerable()
                .FirstOrDefaultAsync(x =>
                    x.Name.ToLowerInvariant().LevenshteinDistance(name) < 2 ||
                    x.Aliases.Any(y => y.ToLowerInvariant().LevenshteinDistance(name) < 2))
                .ConfigureAwait(false);
        }

        public async Task<(bool, Set)> AddAliasAsync(string set, string alias)
        {
            var dbSet = await GetSetAsync(set).ConfigureAwait(false);

            if (dbSet.Aliases.Contains(alias))
                return (false, dbSet);

            dbSet.Aliases.Add(alias);

            try
            {
                Context.Update(dbSet);
                await Context.SaveChangesAsync().ConfigureAwait(false);

                return (true, dbSet);
            }
            catch (DBConcurrencyException)
            {
                return (false, dbSet);
            }
        }

        public async Task<(bool, Set)> RemoveAliasAsync(string set, string alias)
        {
            var dbSet = await GetSetAsync(set).ConfigureAwait(false);

            if (!dbSet.Aliases.Contains(alias))
                return (false, dbSet);

            dbSet.Aliases.Remove(alias);

            try
            {
                Context.Update(dbSet);
                await Context.SaveChangesAsync().ConfigureAwait(false);

                return (true, dbSet);
            }
            catch (DBConcurrencyException)
            {
                return (false, dbSet);
            }
        }
    }
}