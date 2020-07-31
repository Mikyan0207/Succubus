﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using NLog;
using Succubus.Attributes;
using Succubus.Commands.Nsfw.Services;
using Succubus.Database.Models;
using Succubus.Database.Options;
using Succubus.Utils;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Discord.Color;
using Image = Succubus.Database.Models.Image;

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

            Client.ReactionAdded += Client_ReactionAdded;
            Client.ReactionRemoved += Client_ReactionRemoved;
        }

        private async Task Client_ReactionRemoved(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if ((reaction.Emote.Name != "❤️") || reaction.User.Value.IsBot)
                return;

            var embedBuilder = new EmbedBuilder();
            var msg = await message.GetOrDownloadAsync().ConfigureAwait(false);
            var embed = msg.Embeds.FirstOrDefault();
            var set = embed.Footer.GetValueOrDefault().Text.Split('-')[0].Trim();
            var number = int.Parse(embed.Footer.GetValueOrDefault().Text.Split('-')[1].Split('/')[0]);

            bool result = await Service.RemoveImageFromCollectionAsync(reaction.User.Value, set, number).ConfigureAwait(false);

            if (result)
            {
                embedBuilder.WithTitle("Image removed");
                embedBuilder.WithColor(new Color(30, 30, 200));
                await channel.SendMessageAsync("", false, embedBuilder.Build()).ConfigureAwait(false);
            }
        }

        private async Task Client_ReactionAdded(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if ((reaction.Emote.Name != "❤️" && reaction.Emote.Name != "❌") || reaction.User.Value.IsBot)
                return;

            var embedBuilder = new EmbedBuilder();
            var msg = await message.GetOrDownloadAsync().ConfigureAwait(false);
            var embed = msg.Embeds.FirstOrDefault();
            var set = embed.Footer.GetValueOrDefault().Text.Split('-')[0].Trim();
            var number = int.Parse(embed.Footer.GetValueOrDefault().Text.Split('-')[1].Split('/')[0]);

            if (reaction.Emote.Name == "❤️")
            {
                bool result = await Service.AddImageToCollectionAsync(reaction.User.Value, set, number).ConfigureAwait(false);

                if (result)
                {
                    embedBuilder.WithTitle("Image added");
                    embedBuilder.WithColor(new Color(30, 250, 30));
                    await channel.SendMessageAsync("", false, embedBuilder.Build()).ConfigureAwait(false);
                }
            }
            else
            {
                bool result = await Service.RemoveImageFromCollectionAsync(reaction.User.Value, set, number).ConfigureAwait(false);

                if (result)
                {
                    embedBuilder.WithTitle("Image removed");
                    embedBuilder.WithColor(new Color(30, 30, 200));
                    await channel.SendMessageAsync("", false, embedBuilder.Build()).ConfigureAwait(false);
                }
            }

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
            cosplayer.Sets.OrderBy(x => x.Name).ToList().ForEach(x => sb.AppendLine($"{x.Name}"));

            embed.Footer = new EmbedFooterBuilder
            {
                Text = $@"Requested by {Context.Message.Author.Username}"
            };

            embed.WithTitle($"{cosplayer.Name}");
            embed.WithThumbnailUrl($"{cosplayer.ProfilePicture}");
            embed.WithCurrentTimestamp();
            embed.WithColor(new Color(156, 39, 176));

            embed.AddField($"Sets", $"{cosplayer.Sets.Count}", true);
            embed.AddField($"Images", $"{totalPictures}", false);
            embed.AddField($"Twitter", $"[Link]({cosplayer.Twitter})", true);
            embed.AddField($"Instagram", $"[Link]({cosplayer.Instagram})", true);
            embed.AddField($"Booth", $"[Link]({cosplayer.Booth})", true);
            embed.AddField($"Collection", $"{sb}", false);

            await ReplyAsync("", false, embed.Build()).ConfigureAwait(false);
        }

        [Command("yabai", RunMode = RunMode.Async)]
        [Options(typeof(YabaiOptions))]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task YabaiAsync(params string[] options)
        {
            EmbedBuilder embed = new EmbedBuilder();
            Image image = await Service.GetImageAsync(OptionsParser.Parse<YabaiOptions>(options)).ConfigureAwait(false);

            if (image == null)
            {
                embed.WithTitle("No image found");
                embed.WithColor(new Color(255, 30, 30));
                await ReplyAsync("", false, embed.Build()).ConfigureAwait(false);

                return;
            }

            embed.Author = new EmbedAuthorBuilder
            {
                IconUrl = image.Cosplayer.ProfilePicture,
                Url = image.Cosplayer.Twitter,
                Name = image.Cosplayer.Name
            };

            embed.Footer = new EmbedFooterBuilder
            {
                Text = $"{image.Set.Name} - {String.Format("{0:000}", image.Number)}/{String.Format("{0:000}", image.Set.Size)}"
            };

            embed.WithImageUrl(image.Url);
            embed.WithCurrentTimestamp();
            embed.WithColor(new Color(255, 255, 255));

            var msg = await ReplyAsync("", false, embed.Build()).ConfigureAwait(false);

            await msg.AddReactionsAsync(new Emoji[]
            {
                new Emoji("❤️"),
                new Emoji("❌")
            }).ConfigureAwait(false);
        }
    }
}
