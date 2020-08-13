using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Succubus.Database.Models
{
    public class Entity
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public Guid Id { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonIgnore]
        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}