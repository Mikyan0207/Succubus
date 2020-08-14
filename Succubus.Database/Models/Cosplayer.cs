using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Succubus.Database.Models
{
    public class Cosplayer : Entity
    {
        [JsonIgnore]
        public string Name { get; set; }

        public List<string> Keywords { get; set; }

        public List<Set> Sets { get; set; }

        public List<Social> Socials { get; set; }
    }
}