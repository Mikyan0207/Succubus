using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Succubus.Database.Models
{
    public class Cosplayer : Entity
    {
        [JsonIgnore]
        public string Name { get; set; }

        public IReadOnlyCollection<string> Keywords { get; set; }

        public IReadOnlyCollection<Set> Sets { get; set; }

        public IReadOnlyCollection<Social> Socials { get; set; }
    }
}