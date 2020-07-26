using Succubus.Store.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Succubus.Store
{
    public class NamedResourceStore<T> : ResourceStore<T> where T : class
    {
        public string Name { get; set; }

        public NamedResourceStore(IResourceStore<T> store, string name) : base(store)
        {
            Name = name;
        }

        protected override IEnumerable<string> GetFilenames(string name)
        {
            return base.GetFilenames($@"{Name}/{name}");
        }

        public override IEnumerable<string> GetResources()
        {
            return base.GetResources().Where(x => x.StartsWith($"{Name}/")).Select(x => x[(Name.Length + 1)..]);
        }
    }
}
