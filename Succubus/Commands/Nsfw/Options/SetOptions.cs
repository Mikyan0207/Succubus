using CommandLine;

namespace Succubus.Commands.Nsfw.Options
{
    public class SetOptions
    {
        [Option('l', "list", Default = false, HelpText = "List all available sets")]
        public bool List { get; set; }

        [Option]
        public string SetName { get; set; }
    }
}