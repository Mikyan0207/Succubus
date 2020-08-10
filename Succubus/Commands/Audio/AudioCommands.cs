using System.Threading.Tasks;
using Discord.Commands;
using Mikyan.Framework.Commands;
using Succubus.Commands.Audio.Services;

namespace Succubus.Commands.Audio
{
    public class AudioCommands : Module<AudioService>
    {
        [Command("Join", RunMode = RunMode.Async)]
        [Summary("Succubus join a voice channel")]
        public async Task JoinAsync()
        {
            await EmbedAsync(await Service.JoinAsync(Context).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Command("Leave", RunMode = RunMode.Async)]
        [Summary("Succubus leave your voice channel")]
        public async Task LeaveAsync()
        {
            await EmbedAsync(await Service.LeaveAsync(Context).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Command("Play")]
        [Summary("Play a song/video")]
        public async Task PlayAsync([Remainder] string query)
        {
            await EmbedAsync(await Service.PlayAsync(Context, query.ToLowerInvariant().Trim()).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Command("Pause")]
        [Summary("Pause a song/video")]
        public async Task PauseAsync()
        {
            await EmbedAsync(await Service.PauseAsync(Context).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Command("Resume")]
        [Summary("Resume a song/video")]
        public async Task ResumeAsync()
        {
            await EmbedAsync(await Service.ResumeAsync(Context).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Command("Stop")]
        [Summary("Stop a song/video")]
        public async Task StopAsync()
        {
            await EmbedAsync(await Service.StopAsync(Context).ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}