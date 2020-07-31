using NLog;
using Succubus.Database.Context;
using Succubus.Database.Extensions;
using Succubus.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            if (color == null)
                return new Discord.Color(0, 0, 0);

            return new Discord.Color(color.Red, color.Green, color.Blue);
        }
    }
}
