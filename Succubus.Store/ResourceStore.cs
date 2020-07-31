using Succubus.Store.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Succubus.Store
{
    public class ResourceStore<T> : IResourceStore<T> where T : class
    {
        private readonly List<IResourceStore<T>> stores = new List<IResourceStore<T>>();

        private readonly List<string> searchExtensions = new List<string>();

        public ResourceStore()
        {
        }

        public ResourceStore(IResourceStore<T> store = null)
        {
            if (store != null)
                AddStore(store);
        }

        public ResourceStore(IResourceStore<T>[] stores)
        {
            foreach (var store in stores.Cast<ResourceStore<T>>())
                AddStore(store);
        }

        public virtual void AddStore(IResourceStore<T> store)
        {
            lock (stores)
                stores.Add(store);
        }

        public virtual void RemoveStore(IResourceStore<T> store)
        {
            lock (stores)
                stores.Remove(store);
        }

        public virtual T Get(string name)
        {
            if (name == null)
                return null;

            var filenames = GetFilenames(name);

            lock (stores)
            {
                foreach (IResourceStore<T> store in stores)
                {
                    foreach (string fname in filenames)
                    {
                        T result = store.Get(fname);

                        if (result != null)
                            return result;
                    }
                }
            }

            return default;
        }

        public virtual async Task<T> GetAsync(string name)
        {
            if (name == null)
                return null;

            var filenames = GetFilenames(name);

            IResourceStore<T>[] localResourceStores;

            lock (stores)
                localResourceStores = stores.ToArray();

            foreach (IResourceStore<T> store in localResourceStores)
            {
                foreach (string fname in filenames)
                {
                    T result = await store.GetAsync(fname);

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

            var filenames = GetFilenames(name);

            lock (stores)
            {
                foreach (IResourceStore<T> store in stores)
                {
                    foreach (string fname in filenames)
                    {
                        var result = store.GetStream(fname);

                        if (result != null)
                            return result;
                    }
                }
            }

            return null;
        }

        public void AddExtension(string extension)
        {
            extension = extension.Trim('.');

            if (!searchExtensions.Contains(extension))
                searchExtensions.Add(extension);
        }

        public virtual IEnumerable<string> GetResources()
        {
            lock (stores)
                return stores.SelectMany(s => s.GetResources()).ExcludeSystemFiles();
        }

        protected virtual IEnumerable<string> GetFilenames(string name)
        {
            yield return name;

            foreach (var ext in searchExtensions)
                yield return $@"{name}.{ext}";
        }

        private bool isDisposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;
                lock (stores)
                    stores.ForEach(s => s.Dispose());
            }
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