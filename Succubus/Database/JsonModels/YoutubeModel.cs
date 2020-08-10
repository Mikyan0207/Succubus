﻿using System.Collections.Generic;

namespace Succubus.Database.JsonModels
{
    public class YoutubeModel
    {
        public string Name { get; set; }
        public List<string> Keywords { get; set; }
        public string ChannelId { get; set; }
        public string Icon { get; set; }
    }
}