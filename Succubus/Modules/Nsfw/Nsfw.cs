﻿using DSharpPlus.CommandsNext;
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
        [Description("Yabai peko")]
        public async Task YabaiAsync(CommandContext ctx, params string[] args)
        {
            var options = OptionsParser.Parse<YabaiOptions>(args);
            var set = await _service.GetSetAsync(options).ConfigureAwait(false);
            var imgNumber = new Random().Next(1, (int)set.Size);

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
    }
}