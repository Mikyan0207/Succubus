using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Succubus.Database.Models
{
    public class Entity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

    }
}
