using System.Collections.Generic;
using System.Linq;

namespace Succubus.Common.Stores
{
    public class NamedStore : ResourceStore
    {
        public string Name { get; set; }

        public NamedStore(Store store, string name) : base(store)
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