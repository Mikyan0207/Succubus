using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Succubus.Common;
using Succubus.Modules.Nsfw.Options;
using Succubus.Modules.Nsfw.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Succubus.Extensions;

namespace Succubus.Modules.Nsfw
{
    [RequireNsfw]
    [RequirePrefixes("!", ShowInHelp = true)]
    public class Nsfw : SuccubusModule
    {
        private readonly NsfwService _service;

        public Nsfw(NsfwService service)
        {
            _service = service;
        }

        [Command("yabai")]
        public async Task YabaiCommandAsync(CommandContext ctx, params string[] args)
        {
            var options = OptionsParser.Parse<YabaiOptions>(args);
            var set = await _service.GetSetAsync(options).ConfigureAwait(false);
            var imgNumber = new Random().Next(1, set.Size);

            await EmbedAsync(ctx,
                new DiscordEmbedBuilder()
                    .WithAuthor(set.Cosplayer.Name, iconUrl: $"{_service.CloudUrl}/{set.Cosplayer.Keywords.Skip(1).FirstOrDefault()}/ProfilePicture.jpg")
                    .WithFooter($"{set.Name} - {imgNumber:000}/{set.Size:000}")
                    .WithImageUrl(
                        $"{_service.CloudUrl}/{set.Cosplayer.Keywords.Skip(1).FirstOrDefault()}/{set.Folder}/{set.Prefix ?? set.Folder}_{imgNumber:000}.jpg")
                    .WithTimestamp(DateTime.Now)
                    .WithColor(DiscordColor.Purple)
            ).ConfigureAwait(false);

            ctx.Message.DeleteAfter("Succubus Auto-Message Delete", TimeSpan.FromSeconds(2));
        }

        #region Sets

        [Command("sets")]
        public async Task ListSetsCommandAsync(CommandContext ctx)
        {
            var sets = await _service.GetSetsAsync().ConfigureAwait(false);
            var enumerable = sets.ToList();

            await EmbedAsync(ctx,
                new DiscordEmbedBuilder()
                    .WithTitle("List of Sets")
                    .WithDescription($"{enumerable.Count} Sets")
                    .AddField("Sets", string.Join(", ", enumerable.Select(x => x.Name)))
            ).ConfigureAwait(false);
        }

        [Command("set-aa")]
        public async Task AddSetAliasCommandAsync(CommandContext ctx, string set, string alias)
        {
            if (string.IsNullOrWhiteSpace(set) || string.IsNullOrWhiteSpace(alias))
                return;

            var (s, result) = await _service
                .CreateSetAliasAsync(set.Trim().ToLowerInvariant(), alias.Trim().ToLowerInvariant())
                .ConfigureAwait(false);

            if (!result)
            {
                await SendErrorAsync(ctx, "[NSFW - Set] Add Alias", $"Set {set} not found").ConfigureAwait(false);
                return;
            }

            await SendConfirmationAsync(ctx, $"[NSFW - Set] `${s.Name}`",  "Aliases: " + string.Join(", ", s.Keywords))
                .ConfigureAwait(false);
        }

        [Command("set-ra")]
        public async Task RemoveSetAliasCommandAsync(CommandContext ctx, string set, string alias)
        {
            if (string.IsNullOrWhiteSpace(set) || string.IsNullOrWhiteSpace(alias))
                return;

            var (s, result) = await _service
                .RemoveSetAliasAsync(set.Trim().ToLowerInvariant(), alias.Trim().ToLowerInvariant())
                .ConfigureAwait(false);

            if (!result)
            {
                await SendErrorAsync(ctx, "[NSFW - Set] Remove Alias", $"Set {set} not found").ConfigureAwait(false);
                return;
            }

            await SendConfirmationAsync(ctx, $"[NSFW - Set] `${s.Name}`", $"Aliases: {string.Join(", ", s.Keywords)}, ~~{alias}~~")
                .ConfigureAwait(false);
        }

        #endregion Sets

        #region Cosplayers

        [Command("cosplayers")]
        public async Task ListCosplayersCommandAsync(CommandContext ctx)
        {
            var cosplayers = await _service.GetCosplayersAsync().ConfigureAwait(false);
            var enumerable = cosplayers.ToList();

            await EmbedAsync(ctx,
                new DiscordEmbedBuilder()
                    .WithTitle("List of Cosplayers")
                    .WithDescription($"{enumerable.Count} Cosplayers")
                    .AddField("Cosplayers", string.Join(", ", enumerable.Select(x => x.Name)))
            ).ConfigureAwait(false);
        }

        [Command("cosplayer-aa")]
        public async Task AddCosplayerAliasCommandAsync(CommandContext ctx, string cosplayer, string alias)
        {
            if (string.IsNullOrWhiteSpace(cosplayer) || string.IsNullOrWhiteSpace(alias))
                return;

            var (c, result) = await _service
                .CreateCosplayerAliasAsync(cosplayer.Trim().ToLowerInvariant(), alias.Trim().ToLowerInvariant())
                .ConfigureAwait(false);

            if (!result)
            {
                await SendErrorAsync(ctx, "[NSFW - Cosplayer] Add Alias", $"Cosplayer {cosplayer} not found").ConfigureAwait(false);
                return;
            }

            await SendConfirmationAsync(ctx, $"[NSFW - Cosplayer] `${c.Name}`", $"Aliases: {string.Join(", ", c.Keywords)}")
                .ConfigureAwait(false);
        }

        [Command("cosplayer-ra")]
        public async Task RemoveCosplayerAliasCommandAsync(CommandContext ctx, string cosplayer, string alias)
        {
            if (string.IsNullOrWhiteSpace(cosplayer) || string.IsNullOrWhiteSpace(alias))
                return;

            var (c, result) = await _service
                .RemoveCosplayerAliasAsync(cosplayer.Trim().ToLowerInvariant(), alias.Trim().ToLowerInvariant())
                .ConfigureAwait(false);

            if (!result)
            {
                await SendErrorAsync(ctx, "[NSFW - Cosplayer] Remove Alias", $"Cosplayer {cosplayer} not found").ConfigureAwait(false);
                return;
            }

            await SendConfirmationAsync(ctx, $"[NSFW - Cosplayer] `${c.Name}`", $"Aliases: {string.Join(", ", c.Keywords)}, ~~{alias}~~")
                .ConfigureAwait(false);
        }

        #endregion Cosplayers
    }
}