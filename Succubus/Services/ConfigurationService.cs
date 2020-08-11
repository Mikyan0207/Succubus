using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Succubus.Common;

namespace Succubus.Services
{
    public class ConfigurationService : IService
    {
        public SuccubusConfiguration Configuration { get; }

        public ConfigurationService(AssemblyName name,string file)
        {
            using var stream = Assembly.Load(name).GetManifestResourceStream($"{name}.{file}");

            if (stream == null)
                throw new NullReferenceException(nameof(stream));

            using var reader = new StreamReader(stream);

            Configuration = JsonConvert.DeserializeObject<SuccubusConfiguration>(reader.ReadToEnd());
        }
    }
}