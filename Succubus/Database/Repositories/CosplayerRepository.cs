using Microsoft.EntityFrameworkCore;
using Succubus.Database.Context;
using Succubus.Database.Extensions;
using Succubus.Database.Models;
using Succubus.Database.Repositories.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Succubus.Database.Repositories
{
    public class CosplayerRepository : Repository<Cosplayer>, ICosplayerRepository
    {
        public CosplayerRepository(SuccubusContext context) : base(context)
        {
        }

        public async Task<Cosplayer> GetCosplayerAsync(string name)
        {
            return await Context.Cosplayers
                .Include(x => x.Sets)
                .AsAsyncEnumerable()
                .FirstOrDefaultAsync(x =>
                    x.Name.ToLowerInvariant().LevenshteinDistance(name) < 2 ||
                    x.Aliases.Any(y => y.ToLowerInvariant().LevenshteinDistance(name) < 2)).ConfigureAwait(false);
        }
    }
}