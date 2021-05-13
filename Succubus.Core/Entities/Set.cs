using System;
using System.Collections.Generic;
using Succubus.Core.Common;

namespace Succubus.Core.Entities
{
    public class Set : AuditableEntity
    {
        public Set()
        {
            Keywords = new List<string>();
            Images = new List<Image>();
            Cosplayers = new List<Cosplayer>();
        }

        public string Name { get; set; } = string.Empty;

        public List<string> Keywords { get; }

        public int Size { get; set; }

        public int CurrentIndex { get; set; }

        public string Folder { get; set; } = string.Empty;

        public IList<Image> Images { get; }

        public IList<Cosplayer> Cosplayers { get; }
    }
}