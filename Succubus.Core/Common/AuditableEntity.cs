using System;

namespace Succubus.Core.Common
{
    public abstract class AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public DateTime Created { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime Updated { get; set; }

        public string UpdatedBy { get; set; } = string.Empty;
    }
}