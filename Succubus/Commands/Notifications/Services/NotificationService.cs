using System;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.YouTube.v3.Data;
using Succubus.Commands.Notifications.Utils;
using Succubus.Database.Models;
using Succubus.Services;

namespace Succubus.Commands.Notifications.Services
{
    public class NotificationService : IService
    {
        public NotificationService(DbService db)
        {
            DbService = db;
            YoutubeApiService = new YoutubeApiService();
            YoutubeTracker = new YoutubeTracker();
        }

        private YoutubeApiService YoutubeApiService { get; }
        private DbService DbService { get; }
        private YoutubeTracker YoutubeTracker { get; }

        public async Task<LiveResult> IsLive(string channel)
        {
            using var uow = DbService.GetDbContext();
            var ch = await uow.YoutubeChannels.GetChannelByKeyword(channel).ConfigureAwait(false);

            if (ch == null)
                return null;

            var lives = await YoutubeApiService.GetChannelLives(ch.ChannelId).ConfigureAwait(false);

            return lives.FirstOrDefault();
        }

        public async Task<YoutubeChannel> GetChannel(string nameOrId)
        {
            using var uow = DbService.GetDbContext();
            return await uow.YoutubeChannels.GetChannelByKeyword(nameOrId.Trim()).ConfigureAwait(false);
        }

        public Discord.Color GetColor(string name)
        {
            using var uow = DbService.GetDbContext();
            return uow.Colors.GetDiscordColor(name);
        }
    }
}