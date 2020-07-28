using Discord;
using Discord.Commands;
using Discord.WebSocket;
using NLog;
using Succubus.Services.NsfwServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Succubus.Commands.NSFW
{
    [RequireNsfw]
    public class NsfwCommands : ModuleBase<SocketCommandContext>
    {
        private readonly NLog.Logger _Logger;
        private readonly DiscordShardedClient Client;
        private NsfwService NsfwService;

        public NsfwCommands(DiscordShardedClient client)
        {
            Client = client;
            NsfwService = new NsfwService();
            _Logger = LogManager.GetCurrentClassLogger();
        }

        [Command("yabai", RunMode = RunMode.Async)]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        [RequireBotPermission(ChannelPermission.AddReactions)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task SendNsfwImage([Remainder] string options = null)
        {
            if (options == null)
            {
                var image = NsfwService.GetRandomImage();
                await Context.Channel.SendFileAsync(image.Url);
            }
            else if (options == "-s")
            {
                var image = NsfwService.GetRandomImage();
                await Context.Channel.SendFileAsync(image.Url, isSpoiler: true);
            }
            else
            {
            }
        }
    }
}
