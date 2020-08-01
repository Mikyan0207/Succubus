using Discord.Commands;
using Succubus.Services;

namespace Succubus.Commands
{
    public class SuccubusModule<T> : ModuleBase<SocketCommandContext> where T : IService
    {
        public T Service { get; set; }
    }
}