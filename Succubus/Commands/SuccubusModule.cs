using Discord.Commands;
using Succubus.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Succubus.Commands
{
    public class SuccubusModule<T> : ModuleBase<SocketCommandContext> where T : IService
    {
        public T Service { get; set; }

        public SuccubusModule()
        { }
    }
}
