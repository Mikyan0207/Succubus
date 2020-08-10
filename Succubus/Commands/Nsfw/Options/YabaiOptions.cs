using CommandLine;

namespace Succubus.Commands.Nsfw.Options
{
    public class YabaiOptions
    {
        [Option("safe", Default = false, HelpText = "Succubus remove really dangerous images from selection pool.")]
        public bool SafeMode { get; set; }

        [Option('c', "collection", Default = false, HelpText = "Succubus choose a random image from your collection.")]
        public bool FromCollection { get; set; }

        [Option('u', "user", Default = null, HelpText = "Succubus choose a random image from this cosplayer.")]
        public string User { get; set; }

        [Option('s', "set", Default = null, HelpText = "Succubus choose a random image from this set.")]
        public string Set { get; set; }
    }
}