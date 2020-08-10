using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Mikyan.Framework.Commands;
using Mikyan.Framework.Commands.Attributes;
using Mikyan.Framework.Commands.Colors;
using Mikyan.Framework.Commands.Parsers;
using Succubus.Commands.Nsfw.Options;
using Succubus.Commands.Nsfw.Services;
using Succubus.Database.Models;
using Succubus.Services;

namespace Succubus.Commands.Nsfw
{
    [Name("NSFW")]
    [RequireNsfw]
    public class NsfwCommands : Module<NsfwService>
    {
        private readonly DbService _db;
        private readonly LocalizationService _ls;
        private Server _currentGuild;

        public NsfwCommands(LocalizationService localizationService, DbService dbService)
        {
            _ls = localizationService;
            _db = dbService;
        }

        protected override async void BeforeExecute(CommandInfo command)
        {
            _currentGuild = await _db.GetDbContext().Servers.GetOrCreate(Context.Guild).ConfigureAwait(false);
        }

        [Command("Cosplayer", RunMode = RunMode.Async)]
        [Summary("Get information about a Cosplayer present on Succubus")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task CosplayerAsync([Remainder] string name)
        {
            var c = await Service.GetCosplayerAsync(name).ConfigureAwait(false);

            if (c == null)
            {
                await SendErrorAsync("[NSFW] Cosplayer",
                    _ls.GetText("nsfw:cosplayer_not_found",
                        new Dictionary<string, object> { { "Name", name } }, _currentGuild.Locale)).ConfigureAwait(false);
                return;
            }

            await EmbedAsync(
                new EmbedBuilder()
                    .WithTitle($"{c.Name} ({c.Aliases.FirstOrDefault()})")
                    .WithThumbnailUrl($"{Service.BotService.CloudUrl}/{c.ProfilePicture}")
                    .WithCurrentTimestamp()
                    .WithFooter($"Requested by {Context.Message.Author.Username}")
                    .AddField("Sets", $"{c.Sets.Count}", true)
                    .AddField("Images", $"{c.Sets.Sum(x => x.Size)}")
                    .AddField("Twitter", $"[Twitter]({c.Twitter})", true)
                    .AddField("Instagram", $"[Instagram]({c.Instagram})", true)
                    .AddField("Booth", $"[Booth]({c.Booth})", true)
                    .AddField("Collection", string.Join("\n", c.Sets.Select(x => x.Name)))
                    .WithColor(DefaultColors.Purple)
            ).ConfigureAwait(false);
        }

        [Command("Set", RunMode = RunMode.Async)]
        [Summary("Get information about a Set present on Succubus")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        [Options(typeof(SetOptions))]
        public async Task SetAsync(params string[] args)
        {
            var options = OptionsParser.Parse<SetOptions>(args);

            if (options.List)
            {
                var sets = Service.GetSets();
                var eb = new EmbedBuilder()
                    .WithColor(DefaultColors.Info)
                    .WithCurrentTimestamp();

                foreach (var set in sets)
                    eb.AddField($"{set.Name} - {set.Size:000} Images",
                        set.Aliases.Any() ? $"Aliases: `{string.Join(", ", set.Aliases)}`" : "Aliases: -",
                        false);

                await EmbedAsync(eb).ConfigureAwait(false);
                return;
            }

            var name = string.Join(" ", args);
            var s = await Service.GetSetAsync(name).ConfigureAwait(false);

            if (s == null)
            {
                await SendErrorAsync("[NSFW] Set", _ls.GetText("nsfw:set_not_found",
                    new Dictionary<string, object> { { "Name", name } }, _currentGuild.Locale)).ConfigureAwait(false);
                return;
            }

            await EmbedAsync(
                new EmbedBuilder()
                    .WithTitle($"{s.Name} - {s.Size:000} Images")
                    .WithDescription($"{s.Cosplayer.Name}")
                    .WithImageUrl($"{Service.BotService.CloudUrl}/{s.SetPreview}")
                    .WithCurrentTimestamp()
                    .WithColor(DefaultColors.Purple)
            ).ConfigureAwait(false);
        }

        [Command("set-aa", RunMode = RunMode.Async)]
        [Summary("Add an alias to a set")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task AddSetAlias(string set, [Remainder] string alias)
        {
            var (item1, item2) = await Service.AddSetAliasAsync(set.Trim().ToLowerInvariant(), alias.Trim().ToLowerInvariant())
                .ConfigureAwait(false);

            if (item1)
            {
                await SendConfirmationAsync(
                    _ls.GetText("nsfw:set_alias_add_title", new Dictionary<string, object> {{"Name", item2.Name}}, _currentGuild.Locale),
                    _ls.GetText("nsfw:set_alias_add", new Dictionary<string, object> {{"Alias", string.Join(", ", item2.Aliases)}, {"NewAlias", alias}}, _currentGuild.Locale)
                ).ConfigureAwait(false);
            }
        }

        [Command("set-ra", RunMode = RunMode.Async)]
        [Summary("Remove an alias from a set")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task RemoveSetAlias(string set, [Remainder] string alias)
        {
            var (item1, item2) = await Service.RemoveSetAliasAsync(set.Trim().ToLowerInvariant(), alias.Trim().ToLowerInvariant())
                .ConfigureAwait(false);

            if (item1)
            {
                await SendConfirmationAsync(
                    _ls.GetText("nsfw:set_alias_remove_title", new Dictionary<string, object> {{"Name", item2.Name}}, _currentGuild.Locale),
                    _ls.GetText("nsfw:set_alias_remove", new Dictionary<string, object> {{"Alias", string.Join(", ", item2.Aliases)}}, _currentGuild.Locale)
                ).ConfigureAwait(false);
            }
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
                await SendErrorAsync("[NSFW] Yabai", _ls.GetText("nsfw:image_not_found",
                    new Dictionary<string, object>(), _currentGuild.Locale)).ConfigureAwait(false);
                return;
            }

            var imgNumber = new Random().Next(1, (int)set.Size);

            await EmbedAsync(
                new EmbedBuilder()
                    .WithAuthor(set.Cosplayer.Name, $"{Service.BotService.CloudUrl}/{set.Cosplayer.ProfilePicture}",
                        set.Cosplayer.Twitter)
                    .WithFooter($"{set.Name} - {imgNumber:000}/{set.Size:000}")
                    .WithImageUrl(
                        $"{Service.BotService.CloudUrl}/{set.Cosplayer.Aliases.FirstOrDefault()}/{set.FolderName}/{set.FilePrefix ?? set.FolderName}_{imgNumber:000}.jpg")
                    .WithCurrentTimestamp()
                    .WithColor(DefaultColors.Purple)
            ).ConfigureAwait(false);
        }
    }
}