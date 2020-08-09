using Succubus.Bot;
using System.Threading.Tasks;

namespace Succubus
{
    public sealed class Program
    {
        public static async Task Main()
        {
            await new SuccubusBot().RunAsync().ConfigureAwait(false);
        }
    }
}