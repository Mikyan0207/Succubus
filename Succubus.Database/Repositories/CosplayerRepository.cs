using Microsoft.EntityFrameworkCore;
using Succubus.Database.Context;
using Succubus.Database.Extensions;
using Succubus.Database.Models;
using System.Linq;

namespace Succubus.Database.Repositories
{
    public class CosplayerRepository : Repository<Cosplayer>, ICosplayerRepository
    {
        public CosplayerRepository(SuccubusContext context) : base(context)
        {
        }

        public Cosplayer GetCosplayerByName(string name)
        {
            var cosplayer = Context.Cosplayers
                .Include(x => x.Sets)
                .AsEnumerable()
                .Select(x => (x, Distance: x.Name.ToLowerInvariant().LevenshteinDistance(name), AliasDistance: x.Aliases.ToLowerInvariant().LevenshteinDistance(name)))
                .Where(x => x.Distance < 3 || x.AliasDistance < 3)
                .FirstOrDefault().x;

            if (cosplayer == null)
                return null;

            return cosplayer;
        }
    }
}