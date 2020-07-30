using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Succubus.Database.Options
{
    public class YabaiOptions : ICommandOptions
    {
        [Option("safe", Default = false, HelpText = "Succubus remove really dangerous images from selection pool.")]
        public bool SafeMode { get; set; }

        [Option('u', "user", Default = null, HelpText = "Succubus choose a random image from this cosplayer.")]
        public string User { get; set; }

        [Option('s', "set", Default = null, HelpText = "Succubus choose a random image from this set.")]
        public string Set { get; set; }

        public override string ToString()
        {
            return $"User : {(User ?? "null")} | Set : {(Set ?? "null")} | Safe : {SafeMode}";
        }
    }
}
