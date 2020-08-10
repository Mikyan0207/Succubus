using System.Threading.Tasks;
using Discord.Commands;
using Mikyan.Framework.Commands;
using Succubus.Commands.Help.Services;

namespace Succubus.Commands.Help
{
    [Name("Help")]
    public class HelpCommands : Module<HelpService>
    {
        [Command("Help", RunMode = RunMode.Async)]
        [Summary("Get information about a command")]
        public async Task HelpAsync([Remainder] string command)
        {
            var cmd = Service.GetCommand(Context, command);

            if (cmd == null)
            {
                await SendErrorAsync("[Help] Command Help", $"Command {command} not found").ConfigureAwait(false);
                return;
            }

            await EmbedAsync(cmd).ConfigureAwait(false);
        }

        [Command("Module", RunMode = RunMode.Async)]
        [Summary("List all commands available for a specific module")]
        public async Task ModuleAsync(string name)
        {
            await EmbedAsync(Service.GetModuleCommands(name.Trim().ToLowerInvariant())).ConfigureAwait(false);
        }
    }
}