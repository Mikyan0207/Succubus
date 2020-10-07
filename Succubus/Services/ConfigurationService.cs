using Newtonsoft.Json;
using Succubus.Common;
using Succubus.Services.Interfaces;
using System.IO;
using System.Reflection;

namespace Succubus.Services
{
    public class ConfigurationService : IService
    {
        public SuccubusConfiguration Configuration { get; }

        public ConfigurationService()
        {
            var content = File.ReadAllText($"Resources/SuccubusConfiguration.json");

            Configuration = JsonConvert.DeserializeObject<SuccubusConfiguration>(content);
        }
    }
}