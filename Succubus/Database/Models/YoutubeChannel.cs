using System.Collections.Generic;
using Discord;

namespace Succubus.Database.Models
{
    public class YoutubeChannel : Entity
    {
        public string Name { get; set; }

        public List<string> Keywords { get; set; }

        public string ChannelId { get; set; }

        public string Icon { get; set; }

        public ulong RoleId { get; set; }

        public List<DiscordChannel> DiscordChannels { get; set; }
    }
}