using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Mikyan.Framework.Commands.Colors;
using Mikyan.Framework.Services;
using Victoria;
using Victoria.Enums;

namespace Succubus.Commands.Audio.Services
{
    public class AudioService : IService
    {
        private readonly LavaNode _node;

        public AudioService(LavaNode node)
        {
            _node = node;
        }

        public async Task<Embed> JoinAsync(ShardedCommandContext ctx)
        {
            if (_node.HasPlayer(ctx.Guild))
            {
                return new EmbedBuilder()
                    .WithTitle("I'm already connected to a voice channel!")
                    .WithColor(DefaultColors.Error)
                    .Build();
            }

            var voiceState = ctx.User as IVoiceState;

            if (voiceState?.VoiceChannel == null)
            {
                return new EmbedBuilder()
                    .WithTitle("You must be connected to a voice channel!")
                    .WithColor(DefaultColors.Error)
                    .Build();
            }

            try
            {
                await _node.JoinAsync(voiceState.VoiceChannel, ctx.Channel as ITextChannel);

                return new EmbedBuilder()
                    .WithTitle($"Joined {voiceState.VoiceChannel.Name}!")
                    .WithColor(DefaultColors.Info)
                    .Build();
            }
            catch (Exception e)
            {
                return new EmbedBuilder()
                    .WithTitle(e.Message)
                    .WithColor(DefaultColors.Error)
                    .Build();
            }
        }

        public async Task<Embed> LeaveAsync(ShardedCommandContext ctx)
        {
            if (!_node.HasPlayer(ctx.Guild))
            {
                return new EmbedBuilder()
                    .WithTitle("Succubus is not connected!")
                    .WithColor(DefaultColors.Error)
                    .Build();
            }

            var voiceState = ctx.User as IVoiceState;

            if (voiceState?.VoiceChannel == null)
            {
                return new EmbedBuilder()
                    .WithTitle("You must be connected to a voice channel!")
                    .WithColor(DefaultColors.Error)
                    .Build();
            }

            await _node.LeaveAsync(voiceState.VoiceChannel).ConfigureAwait(false);

            return new EmbedBuilder()
                .WithTitle("Bye Bye!")
                .WithColor(DefaultColors.Valid)
                .Build();
        }

        public async Task<Embed> PauseAsync(ShardedCommandContext ctx)
        {
            if (!_node.HasPlayer(ctx.Guild))
            {
                return new EmbedBuilder()
                    .WithTitle("Succubus is not connected!")
                    .WithColor(DefaultColors.Error)
                    .Build();
            }

            var player = _node.GetPlayer(ctx.Guild);

            if (player.PlayerState != PlayerState.Playing)
            {
                return new EmbedBuilder()
                    .WithTitle("No music is playing!")
                    .WithColor(DefaultColors.Error)
                    .Build();
            }

            await player.PauseAsync().ConfigureAwait(false);

            return new EmbedBuilder()
                .WithTitle("Music paused!")
                .WithColor(DefaultColors.Valid)
                .Build();
        }

        public async Task<Embed> ResumeAsync(ShardedCommandContext ctx)
        {
            if (!_node.HasPlayer(ctx.Guild))
            {
                return new EmbedBuilder()
                    .WithTitle("Succubus is not connected!")
                    .WithColor(DefaultColors.Error)
                    .Build();
            }

            var player = _node.GetPlayer(ctx.Guild);

            if (player.PlayerState != PlayerState.Paused)
            {
                return new EmbedBuilder()
                    .WithTitle("Cannot resume")
                    .WithColor(DefaultColors.Error)
                    .Build();
            }

            await player.ResumeAsync().ConfigureAwait(false);

            return new EmbedBuilder()
                .WithTitle("Music resumed!")
                .WithColor(DefaultColors.Valid)
                .Build();
        }

        public async Task<Embed> StopAsync(ShardedCommandContext ctx)
        {
            if (!_node.HasPlayer(ctx.Guild))
            {
                return new EmbedBuilder()
                    .WithTitle("Succubus is not connected!")
                    .WithColor(DefaultColors.Error)
                    .Build();
            }

            var player = _node.GetPlayer(ctx.Guild);

            if (player.PlayerState == PlayerState.Stopped)
            {
                return new EmbedBuilder()
                    .WithTitle("No music is playing!")
                    .WithColor(DefaultColors.Error)
                    .Build();
            }

            await player.StopAsync().ConfigureAwait(false);

            return new EmbedBuilder()
                .WithTitle("Music stopped!")
                .WithColor(DefaultColors.Valid)
                .Build();
        }

        public async Task<Embed> PlayAsync(ShardedCommandContext ctx, string searchQuery)
        {
            if (!_node.HasPlayer(ctx.Guild))
            {
                return new EmbedBuilder()
                    .WithTitle("Succubus is not connected!")
                    .WithColor(DefaultColors.Error)
                    .Build();
            }

            var response = await _node.SearchYouTubeAsync(searchQuery).ConfigureAwait(false);

            if (response.LoadStatus == LoadStatus.LoadFailed || response.LoadStatus == LoadStatus.NoMatches)
            {
                return new EmbedBuilder()
                    .WithTitle($"I wasn't able to find anything for `{searchQuery}`.")
                    .WithColor(DefaultColors.Error)
                    .Build();
            }

            var player = _node.GetPlayer(ctx.Guild);

            if (player.PlayerState == PlayerState.Playing || player.PlayerState == PlayerState.Paused)
            {
                if (!string.IsNullOrWhiteSpace(response.Playlist.Name))
                {
                    foreach (var track in response.Tracks)
                        player.Queue.Enqueue(track);

                    return new EmbedBuilder()
                        .WithTitle($"Enqueued {response.Tracks.Count} tracks.")
                        .WithColor(DefaultColors.Valid)
                        .Build();
                }
                else
                {
                    var track = response.Tracks[0];
                    player.Queue.Enqueue(track);

                    return new EmbedBuilder()
                        .WithTitle($"Enqueued: {track.Title}")
                        .WithColor(DefaultColors.Valid)
                        .Build();
                }
            }
            else
            {
                var track = response.Tracks[0];
                await player.PlayAsync(track);

                return new EmbedBuilder()
                    .WithTitle($"Now Playing: {track.Title}")
                    .WithColor(DefaultColors.Valid)
                    .Build();
            }
        }
    }
}