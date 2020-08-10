using System.ComponentModel.DataAnnotations;

namespace Succubus.Database.Models
{
    public class User : Entity
    {
        [Required] public string Username { get; set; }

        [Required] public string Discriminator { get; set; }

        [Required] public ulong UserId { get; set; }

        public ulong Experience { get; set; }

        public ulong Level { get; set; }

        public override string ToString()
        {
            return $@"{Username}#{Discriminator}";
        }
    }
}