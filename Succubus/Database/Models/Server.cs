using System.ComponentModel.DataAnnotations;

namespace Succubus.Database.Models
{
    public class Server : Entity
    {
        [Required]
        public ulong ServerId { get; set; }

        public string Name { get; set; }

        public string Locale { get; set; }
    }
}