namespace Succubus.Database.Models
{
    public class User : Entity
    {
        public ulong UserId { get; set; }

        public string Username { get; set; }

        public string Discriminator { get; set; }

        public string Avatar { get; set; }

        public override string ToString()
        {
            return $"{Username}#{Discriminator}";
        }
    }
}