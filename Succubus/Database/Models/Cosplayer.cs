using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Succubus.Database.Models
{
    public class Cosplayer : Entity
    {
        [Required] public string Name { get; set; }

        [Required] public List<string> Aliases { get; set; }

        public string ProfilePicture { get; set; }

        public string Twitter { get; set; }

        public string Instagram { get; set; }

        public string Booth { get; set; }

        public List<Set> Sets { get; set; }
    }
}