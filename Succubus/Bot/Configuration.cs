﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Succubus.Bot
{
    public class Configuration
    {
        public string Token { get; set; }

        public ulong BotId { get; set; }

        public string Owner { get; set; }

        public ulong OwnerId { get; set; }

        public int AutoRestart { get; set; }
    }
}
