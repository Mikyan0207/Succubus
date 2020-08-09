using System.Threading.Tasks;
using Succubus.Bot;

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