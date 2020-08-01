using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Succubus.Database.Models;

namespace Succubus.Database.Repositories.Interfaces
{
    public interface IYoutubeChannelRepository : IRepository<YoutubeChannel>
    {
        Task<YoutubeChannel> GetChannelByKeyword(string keyword);

        Task<YoutubeChannel> GetChannel(string nameOrId);
    }
}
