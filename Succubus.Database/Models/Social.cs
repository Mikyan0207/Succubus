namespace Succubus.Database.Models
{
    public class Social : Entity
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public Cosplayer Cosplayer { get; set; }
    }
}