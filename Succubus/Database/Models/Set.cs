using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Succubus.Database.Models
{
    public class Set : Entity
    {
        [Required] public string Name { get; set; }

        public List<string> Aliases { get; set; }

        [Required] public uint Size { get; set; }

        [Required] public string SetPreview { get; set; }

        public string FolderName { get; set; }

        public string FilePrefix { get; set; }

        public Cosplayer Cosplayer { get; set; }

        public YabaiLevel YabaiLevel { get; set; } = YabaiLevel.Safe;
    }
}