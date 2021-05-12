using System;
using Succubus.Core.Common;

namespace Succubus.Core.Entities
{
    public class User : AuditableEntity
    {
        public ulong UserId { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Discriminator { get; set; } = string.Empty;

        public string Avatar { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Username}#{Discriminator}";
        }
    }
}