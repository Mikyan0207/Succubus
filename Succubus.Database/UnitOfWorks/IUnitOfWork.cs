using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Succubus.Database.UnitOfWorks
{
    public interface IUnitOfWork
    {
        int Save();

        Task<int> SaveAsync();
    }
}
