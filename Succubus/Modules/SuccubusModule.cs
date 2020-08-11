using System.Globalization;
using Discord.Commands;
using Discord.Commands.Builders;
using NLog;
using Succubus.Services;

namespace Succubus.Modules
{
    public abstract class SuccubusModule : ModuleBase<ShardedCommandContext>
    {
        protected Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();

        protected LocalizationService LocalizationService { get; set; }

        protected SuccubusModule(LocalizationService ls)
        {
            LocalizationService = ls;
        }

        protected override void OnModuleBuilding(CommandService commandService, ModuleBuilder builder)
        {
            Logger.Info($"Building {builder.Name}");

            foreach (var command in builder.Commands)
            {
                var commandData = LocalizationService.GetCommand(new CultureInfo("fr-FR"), command.Name);

                command.Summary = commandData.Summary;
                command.Remarks = commandData.Remarks;
            }
        }
    }

    public abstract class SuccubusModule<TService> : SuccubusModule where TService : class
    {
        public TService Service { get; set; }

        protected SuccubusModule(LocalizationService ls) : base(ls)
        {
        }
    }
}