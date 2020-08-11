using System.Globalization;
using Discord.Commands;
using Discord.Commands.Builders;
using NLog;
using Succubus.Services;

namespace Succubus.Modules
{
    public abstract class SuccubusModule : ModuleBase<ShardedCommandContext>
    {
    }

    public abstract class SuccubusModule<TService> : SuccubusModule where TService : class
    {
        public TService Service { get; set; }
    }
}