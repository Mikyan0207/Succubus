using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Succubus.Store.Interfaces
{
    public interface IResourceStore<T> : IDisposable
    {
        T Get(string name);

        Task<T> GetAsync(string name);

        Stream GetStream(string name);

        IEnumerable<string> GetResources();
    }

    public static class ResourceStoreExtensions
    {
        private static readonly string[] SYSTEM_FILENAMES =
        {
            "__MACOSX", ".DS_Store", "Thumbs.db"
        };

        public static IEnumerable<string> ExcludeSystemFiles(this IEnumerable<string> list) => list.Where(x => !SYSTEM_FILENAMES.Any(y => x.IndexOf(y, StringComparison.OrdinalIgnoreCase) >= 0));
    }
}
