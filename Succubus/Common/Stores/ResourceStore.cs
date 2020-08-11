using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Succubus.Common.Stores
{
    public class ResourceStore : IDisposable
    {
        private readonly List<Store> _stores = new List<Store>();

        private readonly List<string> _searchExtensions = new List<string>();

        public ResourceStore(Store store = null)
        {
            if (store != null)
                AddStore(store);
        }

        public ResourceStore(IEnumerable<Store> stores)
        {
            foreach (var store in stores)
                AddStore(store);
        }

        public virtual void AddStore(Store store)
        {
            lock (_stores)
                _stores.Add(store);
        }

        public virtual void RemoveStore(Store store)
        {
            lock (_stores)
                _stores.Remove(store);
        }

        public virtual string Get(string name)
        {
            if (name == null)
                return null;

            var fileNames = GetFilenames(name);

            lock (_stores)
            {
                foreach (var result in _stores.SelectMany(store => fileNames, (store, fileName) => store.Get(fileName)).Where(result => result != null))
                {
                    return result;
                }
            }

            return default;
        }

        public virtual async Task<string> GetAsync(string name)
        {
            if (name == null)
                return null;

            var fileNames = GetFilenames(name);

            Store[] localResourceStores;

            lock (_stores)
                localResourceStores = _stores.ToArray();

            var enumerable = fileNames as string[] ?? fileNames.ToArray();
            foreach (var store in localResourceStores)
            {
                foreach (var fileName in enumerable)
                {
                    var result = await store.GetAsync(fileName);

                    if (result != null)
                        return result;
                }
            }

            return default;
        }

        public Stream GetStream(string name)
        {
            if (name == null)
                return null;

            var fileNames = GetFilenames(name);

            lock (_stores)
            {
                foreach (var result in _stores.SelectMany(store => fileNames, (store, fileName) => store.GetStream(fileName)).Where(result => result != null))
                {
                    return result;
                }
            }

            return null;
        }

        public void AddExtension(string extension)
        {
            extension = extension.Trim('.');

            if (!_searchExtensions.Contains(extension))
                _searchExtensions.Add(extension);
        }

        public virtual IEnumerable<string> GetResources()
        {
            lock (_stores)
                return _stores.SelectMany(s => s.GetResources());
        }

        protected virtual IEnumerable<string> GetFilenames(string name)
        {
            yield return name;

            foreach (var ext in _searchExtensions)
                yield return $@"{name}.{ext}";
        }

        private bool _isDisposed;

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
            lock (_stores)
                _stores.ForEach(s => s.Dispose());
        }

        ~ResourceStore()
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