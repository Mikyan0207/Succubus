namespace Succubus.Database.Models
{
    public class Guild : Entity
    {
        public ulong GuildId { get; set; }

        public string Name { get; set; }

        public string Prefix { get; set; }

        public string Locale { get; set; }
    }
}