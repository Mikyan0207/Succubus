using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Succubus.Database.Models
{
    public class Set : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public uint Size { get; set; }

        [Required]
        public string SetPreview { get; set; }

        public List<Image> Images { get; set; }

        public Cosplayer Cosplayer { get; set; }
    }
}
