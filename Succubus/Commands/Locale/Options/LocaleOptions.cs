using CommandLine;

namespace Succubus.Commands.Locale.Options
{
    public class LocaleOptions
    {
        [Option('s', "set", Default = null, HelpText = "Set Locale for your server")]
        public string Locale { get; set; }
    }
}