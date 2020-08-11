using Discord.Commands;
using Succubus.Services;

namespace Succubus.Modules.Help
{
    [Name("Help")]
    public class Help : SuccubusModule
    {
        public Help(LocalizationService ls) : base(ls)
        {
        }
    }
}