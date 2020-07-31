using System;

namespace Succubus.Database.Models
{
    public class UserImage : Entity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid ImageId { get; set; }
        public Image Image { get; set; }
    }
}