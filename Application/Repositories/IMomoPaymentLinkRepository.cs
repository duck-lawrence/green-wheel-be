using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IMomoPaymentLinkRepository
    {
        Task SavePaymentLinkPAsyns(string key, string link);
        Task RemovePaymentLinkAsync(string key);
        Task<string?> GetPaymentLinkAsync(string key);
    }
}
