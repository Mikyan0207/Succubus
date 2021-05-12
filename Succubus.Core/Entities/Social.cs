using System;
using Succubus.Core.Common;

namespace Succubus.Core.Entities
{
    public class Social : AuditableEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public Cosplayer? Cosplayer { get; set; }
    }
}