using Discord.Commands;
using Discord.Commands.Builders;

namespace Succubus.Modules
{
    public abstract class SuccubusModule : ModuleBase<ShardedCommandContext>
    {
        protected override void OnModuleBuilding(CommandService commandService, ModuleBuilder builder)
        {
        }
    }

    public abstract class SuccubusModule<TService> : SuccubusModule where TService : class
    {
        public TService Service { get; set; }
    }
}