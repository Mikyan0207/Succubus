using Discord;
using Discord.Commands;
using Mikyan.Framework.Commands;
using Mikyan.Framework.Commands.Attributes;
using Mikyan.Framework.Commands.Colors;
using Mikyan.Framework.Commands.Parsers;
using Succubus.Commands.Nsfw.Options;
using Succubus.Commands.Nsfw.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Succubus.Commands.Nsfw
{
    [Name("NSFW")]
    [RequireNsfw]
    public class NsfwCommands : Module<NsfwService>
    {
        [Command("Cosplayer", RunMode = RunMode.Async)]
        [Summary("Get information about a Cosplayer present on Succubus")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task CosplayerAsync([Remainder] string name)
        {
            var c = await Service.GetCosplayerAsync(name).ConfigureAwait(false);

            if (c == null)
            {
                await SendErrorAsync("[NSFW] Cosplayer", $"Cosplayer {name} not found").ConfigureAwait(false);
                return;
            }

            await EmbedAsync(
                new EmbedBuilder()
                    .WithTitle($"{c.Name} ({c.Aliases.FirstOrDefault()})")
                    .WithThumbnailUrl($"{Service.BotService.CloudUrl}/{c.ProfilePicture}")
                    .WithCurrentTimestamp()
                    .WithFooter($"Requested by {Context.Message.Author.Username}")
                    .AddField("Sets", $"{c.Sets.Count}", true)
                    .AddField("Images", $"{c.Sets.Sum(x => x.Size)}", false)
                    .AddField("Twitter", $"[Twitter]({c.Twitter})", true)
                    .AddField("Instagram", $"[Instagram]({c.Instagram})", true)
                    .AddField("Booth", $"[Booth]({c.Booth})", true)
                    .AddField("Collection", string.Join("\n", c.Sets.Select(x => x.Name)), false)
                    .WithColor(DefaultColors.Purple)
            ).ConfigureAwait(false);
        }

        [Command("Set", RunMode = RunMode.Async)]
        [Summary("Get information about a Set present on Succubus")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task SetAsync([Remainder] string name)
        {
            var s = await Service.GetSetAsync(name).ConfigureAwait(false);

            if (s == null)
            {
                await SendErrorAsync("[NSFW] Set", $"Set {name} not found").ConfigureAwait(false);
                return;
            }

            await EmbedAsync(
                new EmbedBuilder()
                    .WithTitle($"{s.Name} - {s.Size:000} Images")
                    .WithDescription($"by {s.Cosplayer.Name}")
                    .WithImageUrl($"{Service.BotService.CloudUrl}/{s.SetPreview}")
                    .WithCurrentTimestamp()
                    .WithColor(DefaultColors.Purple)
            ).ConfigureAwait(false);
        }

        [Command("Yabai", RunMode = RunMode.Async)]
        [Summary("Get a random image from Succubus Database")]
        [Options(typeof(YabaiOptions))]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task YabaiAsync(params string[] options)
        {
            var set = await Service.GetSetAsync(OptionsParser.Parse<YabaiOptions>(options)).ConfigureAwait(false);

            if (set == null)
            {
                await SendErrorAsync("[NSFW] Yabai", "No Image found").ConfigureAwait(false);
                return;
            }

            var imgNumber = new Random().Next(1, (int)set.Size);

            await EmbedAsync(
                new EmbedBuilder()
                    .WithAuthor(set.Cosplayer.Name, $"{Service.BotService.CloudUrl}/{set.Cosplayer.ProfilePicture}", set.Cosplayer.Twitter)
                    .WithFooter($"{set.Name} - {imgNumber:000}/{set.Size:000}")
                    .WithImageUrl($"{Service.BotService.CloudUrl}/{set.Cosplayer.Aliases.FirstOrDefault()}/{set.FolderName}/{set.FilePrefix ?? set.FolderName}_{imgNumber:000}.jpg")
                    .WithCurrentTimestamp()
                    .WithColor(DefaultColors.Purple)
            ).ConfigureAwait(false);
        }
    }
}