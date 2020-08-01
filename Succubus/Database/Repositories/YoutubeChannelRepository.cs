using System.Linq;
using System.Threading.Tasks;
using Succubus.Database.Context;
using Succubus.Database.Extensions;
using Succubus.Database.Models;
using Succubus.Database.Repositories.Interfaces;

namespace Succubus.Database.Repositories
{
    public class YoutubeChannelRepository : Repository<YoutubeChannel>, IYoutubeChannelRepository
    {
        public YoutubeChannelRepository(SuccubusContext context) : base(context)
        {
        }

        public async Task<YoutubeChannel> GetChannel(string nameOrId)
        {
            return await Context.YoutubeChannels
                .FirstOrDefaultAsync(x =>
                    x.Name.ToLowerInvariant().LevenshteinDistance(nameOrId) < 2 || x.ChannelId == nameOrId)
                .ConfigureAwait(false);
        }

        public async Task<YoutubeChannel> GetChannelByKeyword(string keyword)
        {
            return await Context.YoutubeChannels
                .FirstOrDefaultAsync(x => x.Keywords.Any(y => y.ToLowerInvariant().LevenshteinDistance(keyword) < 2))
                .ConfigureAwait(false);
        }
    }
}