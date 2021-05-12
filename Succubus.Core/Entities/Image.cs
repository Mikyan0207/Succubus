using System;
using Succubus.Core.Common;

namespace Succubus.Core.Entities
{
    public class Image : AuditableEntity
    {
        public string Name { get; set; } = string.Empty;

        public int Number { get; set; }

        public string File { get; set; } = string.Empty;

        public string Folder { get; set; } = string.Empty;

        public string AbsolutePath { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public Set? Set { get; set; }

        public Cosplayer? Cosplayer { get; set; }
    }
}