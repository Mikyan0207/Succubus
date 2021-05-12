using System;
using System.Collections.Generic;
using Succubus.Core.Common;

namespace Succubus.Core.Entities
{
    public class Cosplayer : AuditableEntity
    {
        public Cosplayer()
        {
            Keywords = new List<string>();
        }

        public string Name { get; set; } = string.Empty;

        public IList<string> Keywords { get; }
    }
}