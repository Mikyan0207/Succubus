using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Succubus.Database.Models
{
    public class Set : Entity
    {
        [JsonIgnore]
        public string Name { get; set; }

        public IReadOnlyCollection<string> Keywords { get; set; }

        public int Size { get; set; }

        public string Folder { get; set; }

        public string Prefix { get; set; }

        public Cosplayer Cosplayer { get; set; }
    }
}