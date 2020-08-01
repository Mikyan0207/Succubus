using System;
using System.Collections.Generic;
using System.Text;

namespace Succubus.Database.Models
{
    public class Role : Entity
    {
        public string Name { get; set; }

        public ulong DiscordId { get; set; }

        public Color Color { get; set; }
    }
}
