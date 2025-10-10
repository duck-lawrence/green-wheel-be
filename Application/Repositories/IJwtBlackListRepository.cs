using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface  IJwtBlackListRepository
    {
        Task SaveTokenAsyns(string key, long ttl);
        Task<bool> CheckTokenInBlackList(string key);
    }
}
