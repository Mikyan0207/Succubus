using System.Threading.Tasks;
using Succubus.Bot;
using Succubus.Logger;

namespace Succubus
{
    public sealed class Program
    {
        public static async Task Main()
        {
            LoggerUtils.InitializeLogger();
            await new SuccubusBot().RunAsync().ConfigureAwait(false);
        }
    }
}