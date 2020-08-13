using CommandLine;

namespace Succubus.Modules.Nsfw.Options
{
    public class YabaiOptions
    {
        [Option("safe", Default = false)]
        public bool SafeMode { get; set; }

        [Option('u', "user", Default = null)]
        public string User { get; set; }

        [Option('s', "set", Default = null)]
        public string Set { get; set; }
    }
}