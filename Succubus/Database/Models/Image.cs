using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Succubus.Database.Models
{
    public class Image : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Url { get; set; }

        public Cosplayer Cosplayer { get; set; }

        public Set Set { get; set; }

        public int Number { get; set; }

        public List<UserImage> Users { get; set; }
    }
}