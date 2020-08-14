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
            var folder = Directory.GetParent(Assembly.GetCallingAssembly().Location)?.Parent?.Parent?.Parent?.FullName;
            var content = File.ReadAllText($"{folder}/Resources/SuccubusConfiguration.json");

            Configuration = JsonConvert.DeserializeObject<SuccubusConfiguration>(content);
        }
    }
}