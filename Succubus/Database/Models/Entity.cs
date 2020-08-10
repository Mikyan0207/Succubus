using System;
using System.ComponentModel.DataAnnotations;

namespace Succubus.Database.Models
{
    public class Entity
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    }
}