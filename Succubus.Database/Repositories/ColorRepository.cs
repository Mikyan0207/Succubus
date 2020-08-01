﻿using NLog;
using Succubus.Database.Context;
using Succubus.Database.Extensions;
using Succubus.Database.Models;
using System.Linq;
using Succubus.Database.Repositories.Interfaces;

namespace Succubus.Database.Repositories
{
    public class ColorRepository : Repository<Color>, IColorRepository
    {
        private static readonly Logger _Logger = LogManager.GetCurrentClassLogger();

        public ColorRepository(SuccubusContext context) : base(context)
        { }

        public Discord.Color GetDiscordColor(string name)
        {
            var color = Context.Colors.FirstOrDefault(x => x.Name.ToLowerInvariant().LevenshteinDistance(name) < 2);

            return color == null ? new Discord.Color(0, 0, 0) : new Discord.Color(color.Red, color.Green, color.Blue);
        }
    }
}