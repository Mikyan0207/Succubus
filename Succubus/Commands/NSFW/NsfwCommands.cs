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
            {
                img = Service.GetRandomImageAsync();
            }
            else
            {
                img = Service.GetRandomImageFromCosplayer(options.Trim());
            }

            if (img == null)
            {
                embed.WithTitle("No image found");
                embed.WithColor(new Color(255, 30, 30));
                await ReplyAsync("", false, embed.Build()).ConfigureAwait(false);
            }
            else
            {
                embed.Author = new EmbedAuthorBuilder
                {
                    IconUrl = img.Cosplayer.ProfilePicture,
                    Url = img.Cosplayer.Twitter,
                    Name = img.Cosplayer.Name
                };

                embed.Footer = new EmbedFooterBuilder
                {
                    Text = $"{img.Set.Name} {String.Format("{0:000}", img.Number)}/{String.Format("{0:000}", img.Set.Size)}"
                };

                embed.WithImageUrl(img.Url);
                embed.WithCurrentTimestamp();
                embed.WithColor(new Color(255, 255, 255));

                await ReplyAsync("", false, embed.Build()).ConfigureAwait(false);
            }

        }
    }
}
