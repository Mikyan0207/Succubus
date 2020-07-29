using Discord;
using Discord.Commands;
using Discord.WebSocket;
using NLog;
using Succubus.Commands.Nsfw.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

        [Command("cosplayer", RunMode = RunMode.Async)]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task CosplayerAsync([Remainder] string name)
        {
            var embed = new EmbedBuilder();

            if (name == null)
            {
                embed.WithTitle("No Cosplayer found");
                embed.WithColor(new Color(255, 30, 30));
                await ReplyAsync("", false, embed.Build()).ConfigureAwait(false);

                return;
            }

            var cosplayer = Service.GetCosplayer(name);

            if (cosplayer == null)
            {
                embed.WithTitle("No Cosplayer found");
                embed.WithColor(new Color(255, 30, 30));
                await ReplyAsync("", false, embed.Build()).ConfigureAwait(false);

                return;
            }

            uint totalPictures = 0;
            cosplayer.Sets.ForEach(x => totalPictures += x.Size);

            StringBuilder sb = new StringBuilder();
            cosplayer.Sets.ForEach(x => sb.AppendLine(x.Name));

            embed.Footer = new EmbedFooterBuilder
            {
                Text = $@"Requested by {Context.Message.Author.Username}"
            };

            embed.WithTitle($"{cosplayer.Name}");
            embed.WithThumbnailUrl($"{cosplayer.ProfilePicture}");
            embed.WithCurrentTimestamp();
            embed.WithColor(new Color(156, 39, 176));

            embed.AddField($"Sets", $"{cosplayer.Sets.Count}", true);
            embed.AddField($"Images", $"{totalPictures}", true);
            embed.AddField($"Twitter", $"[Link]({cosplayer.Twitter})", false);
            embed.AddField($"Instagram", $"[Link]({cosplayer.Instagram})", true);
            embed.AddField($"Booth", $"[Link]({cosplayer.Booth})", true);
            embed.AddField($"Sets Collection", $"{sb}", false);

            await ReplyAsync("", false, embed.Build()).ConfigureAwait(false);
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
                img = Service.GetRandomImage();
            }
            else if (options.StartsWith("-u")) // User
            {
                img = Service.GetRandomImageFromCosplayer(options.Remove(0, 2));
            }
            else if (options.StartsWith("-s")) // Set
            {
                img = Service.GetRandomImageFromSet(options.Remove(0, 2));
            }

            if (img == null)
            {
                embed.WithTitle("No image found");
                embed.WithColor(new Color(255, 30, 30));
                await ReplyAsync("", false, embed.Build()).ConfigureAwait(false);

                return;
            }

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
