using Discord;
using Discord.Commands;
using Discord.WebSocket;
using NLog;
using Succubus.Commands.Nsfw.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Succubus.Commands.Nsfw
{
    [RequireNsfw]
    public class NsfwCommands : SuccubusModule<NsfwService>
    {
        private readonly NLog.Logger _Logger;
        private readonly DiscordShardedClient Client;

        public NsfwCommands(DiscordShardedClient client)
        {
            Client = client;
            _Logger = LogManager.GetCurrentClassLogger();
        }

        [Command("yabai", RunMode = RunMode.Async)]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task YabaiAsync([Remainder] string options = null)
        {
            var embed = new EmbedBuilder();
            Succubus.Database.Models.Image img = null;

            if (options == null)
                img = await Service.GetRandomImageAsync().ConfigureAwait(false);
            else
                img = Service.GetRandomImageFromCosplayer(options);

            embed.Author = new EmbedAuthorBuilder
            {
                IconUrl = img.Cosplayer.ProfilePicture,
                Url = img.Cosplayer.ProfilePicture,
                Name = img.Cosplayer.Name
            };

            embed.Footer = new EmbedFooterBuilder
            {
                Text = $"Image {String.Format("{0:000}",img.Number)}/{String.Format("{0:000}", img.Set.Size)}"
            };

            embed.WithTitle(img.Name);
            embed.WithImageUrl(img.Url);
            embed.WithCurrentTimestamp();
            embed.WithColor(new Color(255, 255, 255));

            await ReplyAsync("", false, embed.Build()).ConfigureAwait(false);
        }
    }
}
