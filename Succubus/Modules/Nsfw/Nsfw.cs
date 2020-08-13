using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
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
            var options = OptionsParser.Parse<YabaiOptions>(args);

        }
    }
}