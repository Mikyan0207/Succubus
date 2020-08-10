using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mikyan.Framework.Extensions;
using Succubus.Database.Context;
using Succubus.Database.Models;
using Succubus.Database.Repositories.Interfaces;

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