using Microsoft.EntityFrameworkCore;
using NLog;
using Succubus.Commands.Nsfw.Options;
using Succubus.Database.Context;
using Succubus.Database.Extensions;
using Succubus.Database.Models;
using Succubus.Database.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Succubus.Database.Repositories
{
    public class SetRepository : Repository<Set>, ISetRepository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public SetRepository(SuccubusContext context) : base(context)
        { }

        public async Task<Set> GetSetAsync(YabaiOptions options)
        {
            return Context.Sets
                .Include(x => x.Cosplayer)
                .ToList()
                .Where(x => options.SafeMode ? x.YabaiLevel == YabaiLevel.Safe : x.YabaiLevel >= YabaiLevel.Safe)
                .ConditionalWhere(options.Set != null, x => x.Name.ToLowerInvariant().LevenshteinDistance(options.Set.ToLowerInvariant()) < 2
                                                            || x.Aliases.Any(y => y.ToLowerInvariant().LevenshteinDistance(options.Set.ToLowerInvariant()) < 2))
                .ConditionalWhere(options.User != null, x => x.Cosplayer.Name.ToLowerInvariant().LevenshteinDistance(options.User) < 2
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
    }
}