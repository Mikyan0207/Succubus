using System;
using System.Collections.Generic;
using System.Text;

namespace Succubus.Database.Models
{
    public class Command : Entity
    {
        public string Name { get; set; }

        public string Alias { get; set; }

        public List<Usage> Usages { get; set; }
    }

    public class Usage
    {
        public User User { get; set; }

        public ulong Count { get; set; }
    }
}
