using Newtonsoft.Json;
using Succubus.Common;
using System.IO;
using System.Reflection;

namespace Succubus.Services
{
    public class ConfigurationService
    {
        public SuccubusConfiguration Configuration { get; }

        public ConfigurationService()
        {
            var content = File.ReadAllText($"Resources/SuccubusConfiguration.json");

            Configuration = JsonConvert.DeserializeObject<SuccubusConfiguration>(content);
        }
    }
}