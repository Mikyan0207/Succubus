using System.Threading.Tasks;

namespace Succubus
{
    internal static class Program
    {
        private static async Task Main()
        {
            await new Succubus().RunAsync().ConfigureAwait(false);
        }
    }
}