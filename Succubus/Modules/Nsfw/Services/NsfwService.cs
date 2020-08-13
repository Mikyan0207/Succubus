using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Succubus.Common.Extensions;
using Succubus.Database.Models;
using Succubus.Modules.Nsfw.Options;
using Succubus.Services;
using Succubus.Services.Interfaces;

namespace Succubus.Modules.Nsfw.Services
{
    public class NsfwService : IService
    {
        private readonly List<Cosplayer> _cosplayers;

        private ConfigurationService Configuration { get; }

        public string CloudUrl => Configuration.Configuration.CloudUrl;

        public NsfwService(ConfigurationService cs)
        {
            Configuration = cs;
            _cosplayers = new List<Cosplayer>();

            var folder = Directory.GetParent(Assembly.GetCallingAssembly().Location)?.Parent?.Parent?.Parent?.FullName;
            var content = File.ReadAllText($"{folder}/Resources/Cosplayers.json");

            _cosplayers = JsonConvert.DeserializeObject<List<Cosplayer>>(content);

            foreach (var cosplayer in _cosplayers)
            {
                cosplayer.Name = cosplayer.Keywords.FirstOrDefault();

                foreach (var set in cosplayer.Sets)
                {
                    set.Cosplayer = cosplayer;
                    set.Name = set.Keywords.FirstOrDefault();
                }
            }
        }

        public Set GetSet(YabaiOptions options)
        {
            return _cosplayers
                .ConditionalWhere(options.User != null, x => x.Keywords.Any(y => y.Equals(options.User)))
                .OrderBy(x => new Random().Next())
                .FirstOrDefault()
                ?.Sets
                .ConditionalWhere(options.Set != null, x => x.Keywords.Any(y => y.Equals(options.Set)))
                .OrderBy(x => new Random().Next())
                .FirstOrDefault();
        }
    }
}