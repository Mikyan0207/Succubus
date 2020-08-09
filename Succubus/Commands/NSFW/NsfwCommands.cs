using System;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Mikyan.Framework.Commands;
using Mikyan.Framework.Commands.Attributes;
using Mikyan.Framework.Commands.Parsers;
using NLog;
using Succubus.Commands.Nsfw.Options;
using Succubus.Commands.Nsfw.Services;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mikyan.Framework.Commands.Colors;

namespace Succubus.Commands.Nsfw
{
    public class NsfwCommands : Module<NsfwService>
    {
        private DiscordShardedClient Client { get; }

        private const string CloudUrl = "https://storage.sbg.cloud.ovh.net/v1/AUTH_9ee93fe3cd9447b4a674a76457cc78a1/SuccubusImages/";

        public NsfwCommands(DiscordShardedClient client)
        {
            Client = client;

            Client.ReactionAdded += Client_ReactionAdded;
            Client.ReactionRemoved += Client_ReactionRemoved;
        }

        private async Task Client_ReactionRemoved(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel,
            SocketReaction reaction)
        {
            if (reaction.Emote.Name != "❤️" || reaction.User.Value.IsBot)
                return;

            var embedBuilder = new EmbedBuilder();
            var msg = await message.GetOrDownloadAsync().ConfigureAwait(false);
            var embed = msg.Embeds.FirstOrDefault();
            var set = embed.Footer.GetValueOrDefault().Text.Split('-')[0].Trim();
            var number = int.Parse(embed.Footer.GetValueOrDefault().Text.Split('-')[1].Split('/')[0]);

            var result = await Service.RemoveImageFromCollectionAsync(reaction.User.Value, set, number)
                .ConfigureAwait(false);

            if (result)
            {
                embedBuilder.WithTitle($"{set} n°{number}");
                embedBuilder.WithDescription($"❌ Image removed from {reaction.User.Value.Username}'s Collection");
                embedBuilder.WithColor(new Color(30, 30, 200));
                await channel.SendMessageAsync("", false, embedBuilder.Build()).ConfigureAwait(false);
            }
        }

        private async Task Client_ReactionAdded(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel,
            SocketReaction reaction)
        {
            if (reaction.Emote.Name != "❤️" && reaction.Emote.Name != "❌" || reaction.User.Value.IsBot)
                return;

            var embedBuilder = new EmbedBuilder();
            var msg = await message.GetOrDownloadAsync().ConfigureAwait(false);
            var embed = msg.Embeds.FirstOrDefault();
            var set = embed.Footer.GetValueOrDefault().Text.Split('-')[0].Trim();
            var number = int.Parse(embed.Footer.GetValueOrDefault().Text.Split('-')[1].Split('/')[0]);

            if (reaction.Emote.Name == "❤️")
            {
                var result = await Service.AddImageToCollectionAsync(reaction.User.Value, set, number)
                    .ConfigureAwait(false);

                if (result)
                {
                    embedBuilder.WithTitle($"{set} n°{number}");
                    embedBuilder.WithDescription($"✅ Image added to {reaction.User.Value.Username}'s Collection");
                    embedBuilder.WithColor(new Color(30, 250, 30));
                    await channel.SendMessageAsync("", false, embedBuilder.Build()).ConfigureAwait(false);
                }
            }
            else
            {
                var result = await Service.RemoveImageFromCollectionAsync(reaction.User.Value, set, number)
                    .ConfigureAwait(false);

                if (result)
                {
                    embedBuilder.WithTitle($"{set} n°{number}");
                    embedBuilder.WithDescription($"❌ Image removed from {reaction.User.Value.Username}'s Collection");
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
            var cosplayer = Service.GetCosplayer(name);

            if (cosplayer == null)
            {
                await SendErrorAsync("[NSFW] Cosplayer", $"Cosplayer {name} not found").ConfigureAwait(false);
                return;
            }

            var embed = new EmbedBuilder();
            uint totalPictures = 0;
            cosplayer.Sets.ForEach(x => totalPictures += x.Size);

            var sb = new StringBuilder();
            cosplayer.Sets.OrderBy(x => x.Name).ToList().ForEach(x => sb.AppendLine($"{x.Name}"));

            embed.Footer = new EmbedFooterBuilder
            {
                Text = $@"Requested by {Context.Message.Author.Username}"
            };

            embed.WithTitle($"{cosplayer.Name}");
            embed.WithThumbnailUrl($"{cosplayer.ProfilePicture}");
            embed.WithCurrentTimestamp();
            embed.WithColor(new Color(156, 39, 176));

            embed.AddField("Sets", $"{cosplayer.Sets.Count}", true);
            embed.AddField("Images", $"{totalPictures}");
            embed.AddField(efb =>
            {
                efb.Name = "";
                efb.Value = !string.IsNullOrEmpty(cosplayer.Twitter) ? $"[Link]({cosplayer.Twitter})" : "None";
                efb.IsInline = true;
            });
            embed.AddField(efb =>
            {
                efb.Name = "";
                efb.Value = !string.IsNullOrEmpty(cosplayer.Instagram) ? $"[Link]({cosplayer.Instagram})" : "None";
                efb.IsInline = true;
            });
            embed.AddField(efb =>
            {
                efb.Name = "";
                efb.Value = !string.IsNullOrEmpty(cosplayer.Booth) ? $"[Link]({cosplayer.Booth})" : "None";
                efb.IsInline = true;
            });
            embed.AddField("Collection", $"{sb}");

            await ReplyAsync("", false, embed.Build()).ConfigureAwait(false);
        }

        [Command("Yabai", RunMode = RunMode.Async)]
        [Options(typeof(YabaiOptions))]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task YabaiAsync(params string[] options)
        {
            var set = await Service.GetSetAsync(OptionsParser.Parse<YabaiOptions>(options)).ConfigureAwait(false);

            if (set == null)
            {
                await SendErrorAsync("[NSFW] Yabai", "No Image found").ConfigureAwait(false);
                return;
            }

            var imgNumber = new Random().Next(1, (int)set.Size);
            var message = await EmbedAsync(
                new EmbedBuilder()
                    .WithAuthor(set.Cosplayer.Name, set.Cosplayer.ProfilePicture, set.Cosplayer.Twitter)
                    .WithFooter($"{set.Name} - {imgNumber:000}/{set.Size:000}")
                    .WithImageUrl($"{CloudUrl}{set.Cosplayer.Aliases.FirstOrDefault()}/{set.FolderName}/{set.FilePrefix ?? set.FolderName}_{imgNumber:000}.jpg")
                    .WithCurrentTimestamp()
                    .WithColor(DefaultColors.Purple)
            ).ConfigureAwait(false);

            await message.AddReactionsAsync(new IEmote[]
            {
                new Emoji("❤️"),
                new Emoji("❌")
            }).ConfigureAwait(false);
        }
    }
}