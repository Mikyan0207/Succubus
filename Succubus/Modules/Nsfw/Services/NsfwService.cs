using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Succubus.Database.Models;
using Succubus.Services;
using Succubus.Services.Interfaces;

namespace Succubus.Modules.Nsfw.Services
{
    public class NsfwService : IService
    {
        private readonly List<Cosplayer> _cosplayers;

        private ConfigurationService Configuration { get; }

        public NsfwService(ConfigurationService cs)
        {
            Configuration = cs;
            _cosplayers = new List<Cosplayer>();

            var folder = Directory.GetParent(Assembly.GetCallingAssembly().Location)?.Parent?.Parent?.Parent?.FullName;
            var content = File.ReadAllText($"{folder}/Resources/Cosplayers.json");

            _cosplayers = JsonConvert.DeserializeObject<List<Cosplayer>>(content);
        }
    }
}