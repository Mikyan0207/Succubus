using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Succubus.Common;
using Succubus.Modules.Nsfw.Options;
using Succubus.Modules.Nsfw.Services;

namespace Succubus.Modules.Nsfw
{
    [RequireNsfw]
    public class Nsfw : SuccubusModule
    {
        private NsfwService _service;

        public Nsfw(NsfwService service)
        {
            _service = service;
        }

        [Command("yabai")]
        public async Task YabaiAsync(CommandContext ctx, params string[] args)
        {
            var sw = new Stopwatch();

            sw.Start();
            var options = OptionsParser.Parse<YabaiOptions>(args);
            var set = _service.GetSet(options);
            var imgNumber = new Random().Next(1, (int)set.Size);

            sw.Stop();

            await EmbedAsync(ctx,
                new DiscordEmbedBuilder()
                    .WithAuthor(set.Cosplayer.Name, iconUrl: $"{_service.CloudUrl}/{set.Cosplayer.Keywords.Skip(1).FirstOrDefault()}/ProfilePicture.jpg")
                    .WithFooter($"{set.Name} - {imgNumber:000}/{set.Size:000} | {sw.Elapsed.TotalMilliseconds:F2}ms")
                    .WithImageUrl(
                        $"{_service.CloudUrl}/{set.Cosplayer.Keywords.Skip(1).FirstOrDefault()}/{set.Folder}/{set.Prefix ?? set.Folder}_{imgNumber:000}.jpg")
                    .WithTimestamp(DateTime.Now)
                    .WithColor(DiscordColor.Purple)
            ).ConfigureAwait(false);
        }
    }
}