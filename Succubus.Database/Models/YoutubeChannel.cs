using System;
using System.Collections.Generic;
using System.Text;

namespace Succubus.Database.Models
{
    public class YoutubeChannel : Entity
    {
        public string Name { get; set; }

        public string UniqueName { get; set; }
    }
}
