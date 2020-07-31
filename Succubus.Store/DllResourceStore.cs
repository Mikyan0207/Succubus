using Succubus.Store.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Succubus.Store
{
    public class DllResourceStore : IResourceStore<byte[]>
    {
        private readonly Assembly assembly;
        private readonly string prefix;

        public DllResourceStore(string dllName)
        {
            string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), dllName);

            assembly = File.Exists(filePath) ? Assembly.LoadFrom(filePath) : Assembly.Load(Path.GetFileNameWithoutExtension(dllName));

            prefix = Path.GetFileNameWithoutExtension(dllName);
        }

        public DllResourceStore(Assembly assembly)
        {
            this.assembly = assembly;
            prefix = assembly.GetName().Name;
        }

        public DllResourceStore(AssemblyName name) : this(Assembly.Load(name))
        {
        }

        public byte[] Get(string name)
        {
            using (Stream input = GetStream(name))
            {
                if (input == null)
                    return null;

                byte[] buffer = new byte[input.Length];
                input.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        public virtual async Task<byte[]> GetAsync(string name)
        {
            using (Stream input = GetStream(name))
            {
                if (input == null)
                    return null;

                byte[] buffer = new byte[input.Length];
                await input.ReadAsync(buffer.AsMemory());
                return buffer;
            }
        }

        public IEnumerable<string> GetResources()
        {
            return assembly.GetManifestResourceNames().Select(n =>
            {
                n = n.Substring(n.StartsWith(prefix) ? prefix.Length + 1 : 0);

                int lastDot = n.LastIndexOf('.');

                var chars = n.ToCharArray();

                for (int i = 0; i < lastDot; i++)
                {
                    if (chars[i] == '.')
                        chars[i] = '/';
                }

                return new string(chars);
            });
        }

        public Stream GetStream(string name)
        {
            var split = name.Split('/');
            for (int i = 0; i < split.Length - 1; i++)
                split[i] = split[i].Replace('-', '_');

            return assembly?.GetManifestResourceStream($@"{prefix}.{string.Join('.', split)}");
        }

        private bool isDisposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;
            }
        }

        ~DllResourceStore()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}