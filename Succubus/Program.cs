using Succubus.Bot;
using System;
using System.Threading.Tasks;

namespace Succubus
{
    public sealed class Program
    {
        public static async Task Main(string[] args)
        {
            await new SuccubusBot().RunAsync().ConfigureAwait(false);
        }
    }
}
