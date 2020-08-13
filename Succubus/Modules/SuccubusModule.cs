using DSharpPlus.CommandsNext;
using Succubus.Services.Interfaces;

namespace Succubus.Modules
{
    public class SuccubusModule : BaseCommandModule
    {
    }

    public class SuccubusModule<TService> : SuccubusModule where TService : IService
    {
        public TService Service { get; set; }
    }
}