using Application.Dtos.Momo.Request;
using Application.Dtos.Momo.Respone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IMomoService
    {
        Task<string> CreatePaymentAsync(CreateMomoPaymentReq req);
        Task VerifyMomoIpnReq(MomoIpnReq req);
    }
}
