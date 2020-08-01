namespace Succubus.Database.Models
{
    public class Color : Entity
    {
        public string Name { get; set; }

        public byte Red { get; set; }

        public byte Green { get; set; }

        public byte Blue { get; set; }
    }
}