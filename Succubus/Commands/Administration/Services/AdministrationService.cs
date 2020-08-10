using Mikyan.Framework.Services;
using Succubus.Database.Context;

namespace Succubus.Commands.Administration.Services
{
    public class AdministrationService : IService
    {
        private SuccubusContext Context { get; }

        public AdministrationService(SuccubusContext context)
        {

        }
    }
}