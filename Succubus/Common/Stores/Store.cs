using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Succubus.Common.Stores
{
    public class Store
    {
        private readonly Assembly _assembly;
        private readonly string _prefix;

        public Store(string dllName)
        {
            var filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location) ?? string.Empty, dllName);

            _assembly = File.Exists(filePath) ? Assembly.LoadFrom(filePath) : Assembly.Load(Path.GetFileNameWithoutExtension(dllName));

            _prefix = Path.GetFileNameWithoutExtension(dllName);
        }

        public Store(Assembly assembly)
        {
            this._assembly = assembly;
            _prefix = assembly.GetName().Name;
        }

        public Store(AssemblyName name) : this(Assembly.Load(name))
        {
        }

        public string Get(string name)
        {
            using var input = GetStream(name);
            if (input == null)
                return null;

            using var reader = new StreamReader(input);
            return reader.ReadToEnd();
        }

        public virtual async Task<string> GetAsync(string name)
        {
            await using var input = GetStream(name);
            if (input == null)
                return null;

            using var reader = new StreamReader(input);
            return await reader.ReadToEndAsync();
        }

        public IEnumerable<string> GetResources()
        {
            return _assembly.GetManifestResourceNames().Select(n =>
            {
                n = n.Substring(n.StartsWith(_prefix) ? _prefix.Length + 1 : 0);

                var lastDot = n.LastIndexOf('.');

                var chars = n.ToCharArray();

                for (var i = 0; i < lastDot; i++)
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
            for (var i = 0; i < split.Length - 1; i++)
                split[i] = split[i].Replace('-', '_');

            return _assembly?.GetManifestResourceStream($@"{_prefix}.{string.Join('.', split)}");
        }

        private bool _isDisposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
            }
        }

        ~Store()
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