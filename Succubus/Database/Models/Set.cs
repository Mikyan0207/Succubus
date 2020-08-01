using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Succubus.Database.Models
{
    public class Set : Entity
    {
        [Required]
        public string Name { get; set; }

        public string Aliases { get; set; }

        [Required]
        public uint Size { get; set; }

        [Required]
        public string SetPreview { get; set; }

        public List<Image> Images { get; set; }

        public Cosplayer Cosplayer { get; set; }

        public YabaiLevel YabaiLevel { get; set; } = YabaiLevel.Safe;
    }

    public enum YabaiLevel
    {
        Safe,
        NotSafe
    }
}