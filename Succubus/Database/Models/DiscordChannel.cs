using System;
using System.Collections.Generic;
using System.Text;

namespace Succubus.Database.Models
{
    public class DiscordChannel
    {
        public string Name { get; set; }

        public ulong Id { get; set; }

        public Server Server { get; set; }

        public bool NotificationActivated { get; set; }
    }
}
